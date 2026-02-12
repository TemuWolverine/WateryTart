using ReactiveUI.Avalonia;
using WateryTart.Platform.Linux.ViewModels;

namespace WateryTart.Platform.Linux.Views;

public partial class GpioVolumeSettingsView : ReactiveUserControl<GpioVolumeSettingsViewModel>
{
    public GpioVolumeSettingsView()
    {
        InitializeComponent();
    }
}