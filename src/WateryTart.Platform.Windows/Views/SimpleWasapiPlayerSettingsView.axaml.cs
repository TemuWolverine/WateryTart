using ReactiveUI.Avalonia;
using WateryTart.Platform.Windows.ViewModels;

namespace WateryTart.Platform.Windows.Views;

public partial class SimpleWasapiPlayerSettingsView : ReactiveUserControl<SimpleWasapiPlayerSettingsViewModel>
{
    public SimpleWasapiPlayerSettingsView()
    {
        InitializeComponent();
    }
}