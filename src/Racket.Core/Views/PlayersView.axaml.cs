using ReactiveUI.Avalonia;
using Racket.Core.ViewModels;

namespace Racket.Core.Views;

public partial class PlayersView : ReactiveUserControl<PlayersViewModel>
{
    public PlayersView()
    {
        InitializeComponent();
    }
}