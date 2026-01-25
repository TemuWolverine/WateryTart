using ReactiveUI;
using WateryTart.MassClient;

namespace WateryTart.ViewModels;

public class PlayersViewModel : ReactiveObject, IViewModelBase
{
    private readonly IMassWsClient _massClient;
    public string? UrlPathSegment { get; }
    public IScreen HostScreen { get; }

    public PlayersViewModel(IMassWsClient massClient, IScreen screen)
    {
        _massClient = massClient;
        HostScreen = screen;
    }

    public string Title { get; set; }
}