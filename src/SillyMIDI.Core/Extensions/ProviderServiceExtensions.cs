using SillyMIDI.Core.Services;
using SillyMIDI.MusicAssistant.Models;

namespace SillyMIDI.Core.Extensions
{
    public static class ProviderServiceExtensions
    {
        public static ProviderManifest? GetProvider(this ProviderService service, string domain)
        {
            return service.ProviderManifests.Find(p => p.Domain == domain);
        }
    }
}
