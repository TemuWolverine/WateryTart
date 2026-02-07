using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;

namespace WateryTart.Core.ViewModels;

public class SettingsViewModel : ReactiveObject, IViewModelBase
{
    private readonly IScreen _screen;
    public ObservableCollection<IHaveSettings> SettingsProviders { get; set; }
    public string? UrlPathSegment { get; } = "settings";
    public IScreen HostScreen => _screen;
    public bool ShowMiniPlayer => false;
    public string Title => "Settings";

    public bool ShowNavigation => true;

    public SettingsViewModel(IEnumerable<IHaveSettings> settingsProviders, IScreen screen)
    {
        _screen = screen;
        SettingsProviders = new ObservableCollection<IHaveSettings>(settingsProviders);
    }
}