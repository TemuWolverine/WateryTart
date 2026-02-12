using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Windows.Input;

namespace WateryTart.Core.Behaviors;

/// <summary>
/// Infinite scroll behavior for grid/WrapPanel layouts
/// </summary>
public class GridScrollBehavior : Behavior<ListBox>
{
    public static readonly StyledProperty<bool> HasMoreItemsProperty = AvaloniaProperty.Register<GridScrollBehavior, bool>(nameof(HasMoreItems), defaultValue: true);
    public static readonly StyledProperty<bool> IsLoadingProperty = AvaloniaProperty.Register<GridScrollBehavior, bool>(nameof(IsLoading));
    public static readonly StyledProperty<ICommand?> LoadMoreCommandProperty = AvaloniaProperty.Register<GridScrollBehavior, ICommand?>(nameof(LoadMoreCommand));

    private double _estimatedItemHeight = 220;
    private bool _hasPerformedInitialCheck = false;
    private bool _hasTriggeredForCurrentBatch = false;
    private int _lastKnownItemCount = 0;
    private int _lastTriggeredAtCount = 0;
    private IDisposable? _offsetSubscription;
    private ScrollViewer? _scrollViewer;

    public bool HasMoreItems
    {
        get => GetValue(HasMoreItemsProperty);
        set => SetValue(HasMoreItemsProperty, value);
    }

    public bool IsLoading
    {
        get => GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    public ICommand? LoadMoreCommand
    {
        get => GetValue(LoadMoreCommandProperty);
        set => SetValue(LoadMoreCommandProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        App.Logger?.LogDebug("[Grid] OnAttached");

        if (AssociatedObject != null)
        {
            AssociatedObject.ContainerPrepared += OnContainerPrepared;
            AssociatedObject.Loaded += OnListBoxLoaded;

            // Also try to find ScrollViewer after a delay
            Avalonia.Threading.DispatcherTimer.RunOnce(() => TryFindScrollViewer(), TimeSpan.FromMilliseconds(500));
        }
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        if (AssociatedObject != null)
        {
            AssociatedObject.ContainerPrepared -= OnContainerPrepared;
            AssociatedObject.Loaded -= OnListBoxLoaded;
        }

        _offsetSubscription?.Dispose();
    }

    private void CheckIfShouldLoadMore()
    {
        App.Logger?.LogDebug("[Grid] CheckIfShouldLoadMore called: Command={CommandExists}, IsLoading={IsLoading}, HasMore={HasMoreItems}, Triggered={HasTriggered}",
            LoadMoreCommand != null, IsLoading, HasMoreItems, _hasTriggeredForCurrentBatch);

        if (LoadMoreCommand == null || IsLoading || !HasMoreItems || _hasTriggeredForCurrentBatch)
            return;

        if (_scrollViewer == null)
        {
            App.Logger?.LogDebug("[Grid] ScrollViewer is null, cannot check");
            return;
        }

        var totalItems = _lastKnownItemCount;
        if (totalItems == 0)
            return;

        var itemsSinceLastTrigger = totalItems - _lastTriggeredAtCount;
        if (_lastTriggeredAtCount > 0 && itemsSinceLastTrigger < 40)
        {
            App.Logger?.LogDebug("[Grid] Not enough items since last trigger: {ItemCount}", itemsSinceLastTrigger);
            return;
        }

        var viewportHeight = _scrollViewer.Viewport.Height;
        var extentHeight = _scrollViewer.Extent.Height;
        var currentScroll = _scrollViewer.Offset.Y;
        var remainingScroll = extentHeight - currentScroll - viewportHeight;
        var estimatedRemainingRows = remainingScroll / _estimatedItemHeight;

        App.Logger?.LogDebug("[Grid] Check: Viewport={ViewportHeight:F0}, Extent={ExtentHeight:F0}, Scroll={CurrentScroll:F0}, Remaining rows={RemainingRows:F1}",
            viewportHeight, extentHeight, currentScroll, estimatedRemainingRows);

        if (estimatedRemainingRows <= 3)
        {
            App.Logger?.LogInformation("[Grid] Triggering Load");
            _hasTriggeredForCurrentBatch = true;
            _lastTriggeredAtCount = totalItems;
            LoadMoreCommand.Execute(null);
        }
    }

    private void OnContainerPrepared(object? sender, ContainerPreparedEventArgs e)
    {
        var totalItems = AssociatedObject?.ItemCount ?? 0;
        if (totalItems == 0)
            return;

        // Try to find ScrollViewer if we don't have it yet
        if (_scrollViewer == null && e.Index == 5)
        {
            App.Logger?.LogDebug("[Grid] Attempting to find ScrollViewer from ContainerPrepared");
            TryFindScrollViewer();
        }

        if (e.Index == 0 && e.Container is Control container)
        {
            container.LayoutUpdated += (s, args) =>
            {
                if (container.Bounds.Height > 0 && container.Bounds.Height != _estimatedItemHeight)
                {
                    _estimatedItemHeight = container.Bounds.Height;
                    App.Logger?.LogDebug("[Grid] Item height: {ItemHeight}", _estimatedItemHeight);
                }
            };
        }

        if (totalItems > _lastKnownItemCount)
        {
            _lastKnownItemCount = totalItems;
            _hasTriggeredForCurrentBatch = false;
            App.Logger?.LogDebug("[Grid] New batch: {ItemCount} items", totalItems);

            if (!_hasPerformedInitialCheck)
            {
                _hasPerformedInitialCheck = true;
                App.Logger?.LogDebug("[Grid] Scheduling initial check...");
                Avalonia.Threading.DispatcherTimer.RunOnce(() => CheckIfShouldLoadMore(), TimeSpan.FromMilliseconds(300));
            }
        }
    }

    private void OnListBoxLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        App.Logger?.LogDebug("[Grid] OnListBoxLoaded fired");
        TryFindScrollViewer();
    }

    private void OnScrollChanged()
    {
        CheckIfShouldLoadMore();
    }

    private void TryFindScrollViewer()
    {
        if (_scrollViewer != null)
        {
            App.Logger?.LogDebug("[Grid] ScrollViewer already found");
            return;
        }

        App.Logger?.LogDebug("[Grid] Attempting to find ScrollViewer...");

        // First, try to find a parent ScrollViewer (more likely to have proper constraints)
        var parent = AssociatedObject?.GetVisualAncestors()
            .OfType<ScrollViewer>()
            .FirstOrDefault();

        if (parent != null)
        {
            _scrollViewer = parent;
            App.Logger?.LogDebug("[Grid] Found PARENT ScrollViewer");
        }
        else
        {
            // Fall back to internal ScrollViewer
            _scrollViewer = AssociatedObject?.GetVisualDescendants()
                .OfType<ScrollViewer>()
                .FirstOrDefault();
            App.Logger?.LogDebug("[Grid] Found internal ScrollViewer (fallback)");
        }

        App.Logger?.LogDebug("[Grid] ScrollViewer found: {Found}", _scrollViewer != null);

        if (_scrollViewer != null)
        {
            App.Logger?.LogDebug("[Grid] Initial Viewport: {Viewport}, Extent: {Extent}", _scrollViewer.Viewport, _scrollViewer.Extent);

            // Check if scrolling is actually possible
            if (_scrollViewer.Viewport.Height >= _scrollViewer.Extent.Height - 1)
            {
                App.Logger?.LogWarning("[Grid] Viewport equals Extent - no scrolling possible! ListBox may not be height-constrained.");
            }

            _offsetSubscription = _scrollViewer.GetObservable(ScrollViewer.OffsetProperty)
                .Subscribe(offset =>
                {
                    App.Logger?.LogDebug("[Grid] Scroll offset changed: {Offset}", offset.Y);
                    OnScrollChanged();
                });

            // Also observe Extent changes (when items load)
            _scrollViewer.GetObservable(ScrollViewer.ExtentProperty)
                .Subscribe(extent =>
                {
                    App.Logger?.LogDebug("[Grid] Extent changed: {ExtentHeight}, Viewport: {ViewportHeight}", extent.Height, _scrollViewer.Viewport.Height);
                });
        }
        else
        {
            App.Logger?.LogWarning("[Grid] ScrollViewer NOT found in visual tree");
            App.Logger?.LogDebug("[Grid] AssociatedObject: {ObjectType}", AssociatedObject?.GetType().Name);
        }
    }
}