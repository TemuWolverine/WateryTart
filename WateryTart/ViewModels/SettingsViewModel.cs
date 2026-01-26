using ReactiveUI;

namespace WateryTart.ViewModels;

public class SettingsViewModel : ReactiveObject, IViewModelBase
{
    public string? UrlPathSegment { get; }
    public IScreen HostScreen { get; }
    public string Title
    {
        get => "Settings";
        set; }
}