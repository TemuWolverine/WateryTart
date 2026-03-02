using Avalonia.Markup.Xaml.Templates;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System.Reactive.Linq;
using WateryTart.Core.ViewModels;

namespace WateryTart.Core.Views;

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