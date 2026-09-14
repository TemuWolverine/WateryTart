using ReactiveUI.Avalonia;
using SillyMIDI.Core.ViewModels;

namespace SillyMIDI.Core.Views;

public partial class LoginView : ReactiveUserControl<LoginViewModel>
{
    public LoginView()
    {
        InitializeComponent();
    }
}