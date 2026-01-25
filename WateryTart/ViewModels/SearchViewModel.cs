using ReactiveUI;
using ReactiveUI.SourceGenerators;
using WateryTart.MassClient;
using WateryTart.Services;

namespace WateryTart.ViewModels;

public partial class SearchViewModel : ReactiveObject, IViewModelBase
{
    private readonly IMassWsClient _massClient;
    public string? UrlPathSegment { get; }
    public IScreen HostScreen { get; }
    [Reactive] public string Title
    {
        get { return "Search";} set;
    }

    [Reactive] public partial string SearchTerm { get; set; }

    public SearchViewModel(IMassWsClient massClient, IScreen screen)
    {
        _massClient = massClient;
        HostScreen = screen;
    }
}