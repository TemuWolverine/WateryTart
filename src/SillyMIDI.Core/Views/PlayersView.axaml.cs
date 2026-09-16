using ReactiveUI.Avalonia;
using SillyMIDI.Core.ViewModels;

namespace SillyMIDI.Core.Views;

public partial class PlayersView : ReactiveUserControl<PlayersViewModel>
{
    public PlayersView()
    {
        InitializeComponent();
    }
}