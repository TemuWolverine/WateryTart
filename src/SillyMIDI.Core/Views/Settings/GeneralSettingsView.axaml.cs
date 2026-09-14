using ReactiveUI.Avalonia;
using SillyMIDI.Core.ViewModels;

namespace SillyMIDI.Core.Views;

public partial class GeneralSettingsView : ReactiveUserControl<GeneralSettingsViewModel>
{
    public GeneralSettingsView()
    {
        InitializeComponent();
    }
}