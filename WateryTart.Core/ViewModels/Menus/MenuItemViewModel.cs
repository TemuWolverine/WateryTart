using ReactiveUI;
using System.Windows.Input;
using WateryTart.Core.Messages;
using Xaml.Behaviors.SourceGenerators;

namespace WateryTart.Core.ViewModels.Menus;

public partial class MenuItemViewModel : ReactiveObject, ISmallViewModelBase
{
    private readonly ICommand _clickedCommand;
    private readonly string _icon;
    private readonly string _title; public string Icon => _icon;
    public string Title => _title;

    public MenuItemViewModel(string title, string icon, ICommand clickedCommand)
    {
        _title = title;
        _icon = icon;
        _clickedCommand = clickedCommand;
    }

    [GenerateTypedAction]
    public void MenuItemClicked()
    {
        _clickedCommand.Execute(null);
        MessageBus.Current.SendMessage(new CloseMenuMessage());
    }
}