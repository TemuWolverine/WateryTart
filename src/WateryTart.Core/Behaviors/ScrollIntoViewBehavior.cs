using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using WateryTart.Core.ViewModels.Players;

namespace WateryTart.Core.Behaviors
{
    public class ScrollIntoViewBehavior : Behavior<ItemsControl>
    {
        private int _lastIndex = -1;

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject?.DataContext is not BigPlayerViewModel viewModel)
                return;

            var lyricsVm = viewModel.Lyrics;

            // Subscribe to CurrentLineIndex changes on the LyricsViewModel
            lyricsVm.WhenAnyValue(x => x.CurrentLineIndex)
                .Subscribe(index =>
                {
                    if (index >= 0 && index != _lastIndex && AssociatedObject?.Items?.Count > index)
                    {
                        try
                        {
                            _lastIndex = index;
                            var item = AssociatedObject.Items[index];
                            AssociatedObject.ScrollIntoView(item);
                        }
                        catch { }
                    }
                });
        }
    }
}

