using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace WateryTart.ViewModels;

public partial class LibraryViewModel : ReactiveObject, IViewModelBase
{
    public string? UrlPathSegment { get; }
    public IScreen HostScreen { get; }
    [Reactive] public string Title { get; set; }
}