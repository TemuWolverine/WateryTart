using ReactiveUI.Avalonia;
using Racket.Core.ViewModels.Players;

namespace Racket.Core.Views.Players;

public partial class MiniPlayerView : ReactiveUserControl<MiniPlayerViewModel>
{
    public MiniPlayerView()
    {
        InitializeComponent();
    }
}