using IconPacks.Avalonia.Material;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System.Collections.Generic;
using System.Linq;
using WateryTart.Core.ViewModels.Popups;

namespace WateryTart.Core.ViewModels.Menus;

public partial class MenuViewModel(IEnumerable<IMenuItemViewModel>? menuItems = null, object? headerItem = null) : ReactiveObject, IPopupViewModel
{
    [Reactive] public partial object? HeaderItem { get; set; } = headerItem;
    public List<IMenuItemViewModel> MenuItems { get; set; } = menuItems?.ToList() ?? [];
    public string Title { get; set; } = string.Empty;
    public string Message => throw new System.NotImplementedException();

    public void AddMenuItem(IMenuItemViewModel menuItem)
    {
        MenuItems.Add(menuItem);
    }

    public void AddMenuItem(IEnumerable<IMenuItemViewModel?> menuItems)
    {
        foreach (var item in menuItems)
        {
            if (item != null)
                MenuItems.Add(item);
        }
    }
}