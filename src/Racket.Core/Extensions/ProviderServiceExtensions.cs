using Racket.Core.Services;
using WateryTart.MusicAssistant.Models;

namespace Racket.Core.Extensions
{
    public static class ProviderServiceExtensions
    {
        public static ProviderManifest? GetProvider(this ProviderService service, string domain)
        {
            return service.ProviderManifests.Find(p => p.Domain == domain);
        }
    }
}
