using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace WateryTart.ViewModels;

public partial class RecommendationViewModel : ReactiveObject, IViewModelBase
{
    public string? UrlPathSegment { get; }
    public IScreen HostScreen { get; }
    [Reactive] public partial string Title { get; set; }
}