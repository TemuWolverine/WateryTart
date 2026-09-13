using Avalonia.Controls;
using ReactiveUI.Avalonia;
using Racket.Core.ViewModels;

namespace Racket.Core.Views;

public partial class ServerSettingsView : ReactiveUserControl<ServerSettingsViewModel>
{
    public ServerSettingsView()
    {
        InitializeComponent();
    }
}
