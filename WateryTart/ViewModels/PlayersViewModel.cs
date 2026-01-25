using ReactiveUI;
using WateryTart.MassClient;

namespace WateryTart.ViewModels;

public class PlayersViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly IMassWsClient _massClient;
    public string? UrlPathSegment { get; }
    public IScreen HostScreen { get; }

    public PlayersViewModel(IMassWsClient massClient, IScreen screen)
    {
        _massClient = massClient;
        HostScreen = screen;
    }
}