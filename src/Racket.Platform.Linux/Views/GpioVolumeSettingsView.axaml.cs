using ReactiveUI.Avalonia;
using Racket.Platform.Linux.ViewModels;

namespace Racket.Platform.Linux.Views;

public partial class GpioVolumeSettingsView : ReactiveUserControl<GpioVolumeSettingsViewModel>
{
    public GpioVolumeSettingsView()
    {
        InitializeComponent();
    }
}