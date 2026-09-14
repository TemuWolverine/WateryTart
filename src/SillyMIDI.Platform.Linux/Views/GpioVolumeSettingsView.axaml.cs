using ReactiveUI.Avalonia;
using SillyMIDI.Platform.Linux.ViewModels;

namespace SillyMIDI.Platform.Linux.Views;

public partial class GpioVolumeSettingsView : ReactiveUserControl<GpioVolumeSettingsViewModel>
{
    public GpioVolumeSettingsView()
    {
        InitializeComponent();
    }
}