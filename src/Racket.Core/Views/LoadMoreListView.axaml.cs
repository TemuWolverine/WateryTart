using Avalonia.Markup.Xaml.Templates;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System.Reactive.Linq;
using Racket.Core.ViewModels;

namespace Racket.Core.Views;

public partial class LoadMoreListView : ReactiveUserControl<ILoadMoreListViewModel>
{

    public LoadMoreListView()
    {
        InitializeComponent();

        _ = this.WhenActivated(disposables =>
        {
            if (ViewModel != null)
                ApplyItemsPanel(ViewModel.UseWrapPanel);
        });
    }

    private void ApplyItemsPanel(bool useWrap)
    {
        var key = useWrap ? "WrapPanelTemplate" : "StackPanelTemplate";
        if (this.Resources.TryGetResource(key, ActualThemeVariant.InheritVariant, out var template) && template is ItemsPanelTemplate ipt)
        {
            ItemsListBox.ItemsPanel = ipt;
        }
    }
}