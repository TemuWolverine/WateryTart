using Avalonia.Controls;
using ReactiveUI.Avalonia;
using SillyMIDI.Core.ViewModels;

namespace SillyMIDI.Core.Views;

public partial class ServerSettingsView : ReactiveUserControl<ServerSettingsViewModel>
{
    public ServerSettingsView()
    {
        InitializeComponent();
    }
}
