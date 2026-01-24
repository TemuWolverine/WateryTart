using ReactiveUI;
using WateryTart.MassClient;
using WateryTart.Settings;

namespace WateryTart.ViewModels
{
    public class HomeViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly IMassWSClient _massClient;
        private readonly ISettings _settings;
        private readonly IScreen _screen;

        public HomeViewModel(IScreen screen, IMassWSClient massClient, ISettings settings)
        {
            _massClient = massClient;
            _settings = settings;
            _screen = screen;

            //{
            // "command": "music/recommendations"
        }

        public string? UrlPathSegment { get; }
        public IScreen HostScreen { get; }
    }
}