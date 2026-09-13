using ReactiveUI.Avalonia;
using Racket.Core.ViewModels;

namespace Racket.Core.Views;

public partial class LoginView : ReactiveUserControl<LoginViewModel>
{
    public LoginView()
    {
        InitializeComponent();
    }
}