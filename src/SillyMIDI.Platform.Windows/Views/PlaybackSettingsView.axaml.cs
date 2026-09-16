using ReactiveUI.Avalonia;
using SillyMIDI.Platform.Windows.ViewModels;

namespace SillyMIDI.Platform.Windows.Views;

public partial class PlaybackSettingsView : ReactiveUserControl<PlaybackSettingsViewModel>
{
    public PlaybackSettingsView()
    {
        InitializeComponent();
    }
}
