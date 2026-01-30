using ReactiveUI.Avalonia;
using WateryTart.ViewModels.Players;

namespace WateryTart.Views.Players;

public partial class BigPlayerView : ReactiveUserControl<BigPlayerViewModel>
{
    public BigPlayerView()
    {
        InitializeComponent();
    }
}