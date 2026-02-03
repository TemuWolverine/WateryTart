using Avalonia.Controls;
using ReactiveUI.Avalonia;
using WateryTart.Core.ViewModels;

namespace WateryTart.Core.Views;

public partial class TracksView : ReactiveUserControl<TracksViewModel>
{
    public TracksView()
    {
        InitializeComponent();
    }
}