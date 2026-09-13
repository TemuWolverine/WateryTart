using ReactiveUI.Avalonia;
using Racket.Platform.Windows.ViewModels;

namespace Racket.Platform.Windows.Views;

public partial class PlaybackSettingsView : ReactiveUserControl<PlaybackSettingsViewModel>
{
    public PlaybackSettingsView()
    {
        InitializeComponent();
    }
}
