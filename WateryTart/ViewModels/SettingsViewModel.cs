using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace WateryTart.ViewModels;

public partial class SettingsViewModel : ReactiveObject, IViewModelBase
{
    public string? UrlPathSegment { get; }
    public IScreen HostScreen { get; }
    [Reactive] public string Title
    {
        get { return "Settings";} set; }
}