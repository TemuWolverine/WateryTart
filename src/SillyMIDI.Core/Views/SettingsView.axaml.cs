using ReactiveUI.Avalonia;
using SillyMIDI.Core.ViewModels;

namespace SillyMIDI.Core.Views;

public partial class SettingsView : ReactiveUserControl<SettingsViewModel>
{
    public SettingsView()
    {
        InitializeComponent();
    }
}