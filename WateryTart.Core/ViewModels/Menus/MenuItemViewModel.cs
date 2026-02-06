using ReactiveUI;
using System.Windows.Input;
using WateryTart.Core.Messages;
using Xaml.Behaviors.SourceGenerators;

namespace WateryTart.Core.ViewModels.Menus;

public partial class MenuItemViewModel : ReactiveObject, IViewModelBase
{
    private readonly ICommand _clickedCommand;
    public string? UrlPathSegment { get; }
    public IScreen HostScreen { get; }
    public string Title { get; set; } = string.Empty;
    public string Icon { get; } = string.Empty;
    public bool ShowMiniPlayer => false;
    public bool ShowNavigation => false;

    [GenerateTypedAction]
    public void MenuItemClicked()
    {
        _clickedCommand.Execute(null);
        MessageBus.Current.SendMessage(new CloseMenuMessage());
    }
    public MenuItemViewModel(string title, string icon, ICommand clickedCommand)
    {
        _clickedCommand = clickedCommand;
        Title = title;
        Icon = icon;
    }
}