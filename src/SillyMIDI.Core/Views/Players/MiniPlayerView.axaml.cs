using ReactiveUI.Avalonia;
using SillyMIDI.Core.ViewModels.Players;

namespace SillyMIDI.Core.Views.Players;

public partial class MiniPlayerView : ReactiveUserControl<MiniPlayerViewModel>
{
    public MiniPlayerView()
    {
        InitializeComponent();
    }
}