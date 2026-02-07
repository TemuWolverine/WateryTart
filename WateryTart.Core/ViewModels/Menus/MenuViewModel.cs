using ReactiveUI;
using System.Collections.Generic;
using System.Linq;

namespace WateryTart.Core.ViewModels.Menus;

public partial class MenuViewModel : ReactiveObject, ISmallViewModelBase
{
    public List<MenuItemViewModel> MenuItems { get; set; } = new List<MenuItemViewModel>();
    public bool ShowMiniPlayer => false;
    public bool ShowNavigation => false;
    public string Title { get; set; } = string.Empty;
    public string Icon { get; } = string.Empty;
    public string? UrlPathSegment { get; } = string.Empty;

    public MenuViewModel(IEnumerable<MenuItemViewModel>? menuItems = null)
    {
        MenuItems = menuItems?.ToList() ?? [];
    }

    public void AddMenuItem(MenuItemViewModel menuItem)
    {
        MenuItems.Add(menuItem);
    }

    public void AddMenuItem(IEnumerable<MenuItemViewModel?> menuItems)
    {
        foreach (var item in menuItems)
        {
            if (item != null)
                MenuItems.Add(item);
        }
    }
}