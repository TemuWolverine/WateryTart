using ReactiveUI.Avalonia;
using Racket.Core.ViewModels;

namespace Racket.Core.Views;

public partial class GeneralSettingsView : ReactiveUserControl<GeneralSettingsViewModel>
{
    public GeneralSettingsView()
    {
        InitializeComponent();
    }
}