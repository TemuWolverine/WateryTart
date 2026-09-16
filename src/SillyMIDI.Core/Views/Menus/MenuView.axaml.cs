using ReactiveUI.Avalonia;
using SillyMIDI.Core.ViewModels.Menus;

namespace SillyMIDI.Core.Views.Menus;

public partial class MenuView : ReactiveUserControl<MenuViewModel>
{
    public MenuView()
    {
        InitializeComponent();
    }
}