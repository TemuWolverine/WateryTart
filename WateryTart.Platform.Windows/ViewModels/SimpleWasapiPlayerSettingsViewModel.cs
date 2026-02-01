using ReactiveUI;
using WateryTart.Core;
using WateryTart.Core.ViewModels;

namespace WateryTart.Platform.Windows.ViewModels
{
    public class SimpleWasapiPlayerSettingsViewModel : ReactiveObject, IViewModelBase, IHaveSettings
    {
        public string? UrlPathSegment { get; }
        public IScreen HostScreen { get; }
        public string Title { get; set; }
        public bool ShowMiniPlayer { get; }
        public bool ShowNavigation { get; }
        public string Icon => "Speaker";
    }
}
