using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;

namespace WateryTart.Core.ViewModels;

public class SettingsViewModel : ReactiveObject, IViewModelBase
{
    public ObservableCollection<IHaveSettings> SettingsProviders { get; set; }
    public string? UrlPathSegment { get; }
    public IScreen HostScreen { get; }
    public bool ShowMiniPlayer { get => false; }
    public string Title
    {
        get => "Settings";
        set;
    }

    public bool ShowNavigation => true;

    public SettingsViewModel(IEnumerable<IHaveSettings> settingsProviders)
    {
        SettingsProviders = new ObservableCollection<IHaveSettings>(settingsProviders);
    }
}