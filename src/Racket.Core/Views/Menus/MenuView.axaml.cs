using ReactiveUI.Avalonia;
using Racket.Core.ViewModels.Menus;

namespace Racket.Core.Views.Menus;

public partial class MenuView : ReactiveUserControl<MenuViewModel>
{
    public MenuView()
    {
        InitializeComponent();
    }
}