using ReactiveUI.Avalonia;
using WateryTart.ViewModels;

namespace WateryTart.Views;

public partial class LoginView : ReactiveUserControl<LoginViewModel>
{
    public LoginView()
    {
        InitializeComponent();
    }
}