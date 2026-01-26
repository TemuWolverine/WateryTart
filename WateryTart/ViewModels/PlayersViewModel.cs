using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using WateryTart.MassClient;
using WateryTart.MassClient.Models;
using WateryTart.Services;

namespace WateryTart.ViewModels;

public partial class PlayersViewModel : ReactiveObject, IViewModelBase
{
    private readonly IMassWsClient _massClient;
    private readonly IPlayersService _playersService;
    public string? UrlPathSegment { get; }
    public IScreen HostScreen { get; }

    [Reactive] public partial ObservableCollection<Player> Players { get; set; }

    public PlayersViewModel(IMassWsClient massClient, IScreen screen, IPlayersService playersService)
    {
        _massClient = massClient;
        _playersService = playersService;
        HostScreen = screen;
        Players = playersService.Players;
    }

    public string Title
    {
        get => "Players";
        set; }
}