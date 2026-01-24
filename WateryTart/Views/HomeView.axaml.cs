using ReactiveUI.Avalonia;
using WateryTart.ViewModels;

namespace WateryTart.Views;

public partial class HomeView : ReactiveUserControl<HomeViewModel>
{
    public HomeView()
    {
        InitializeComponent();
    }
}