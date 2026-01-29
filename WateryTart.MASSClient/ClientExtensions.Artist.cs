using Newtonsoft.Json;
using System.Collections;
using WateryTart.MassClient.Messages;
using WateryTart.MassClient.Responses;

namespace WateryTart.MassClient;


public static partial class MassClientExtensions
{
    extension(IMassWsClient c)
    {

        public void ArtistGet(string artistid, string provider_instance_id_or_domain, Action<ArtistResponse> responseHandler)
        {
            c.Send<ArtistResponse>(IdAndProvider(Commands.ArtistGet, artistid, provider_instance_id_or_domain), Deserialise<ArtistResponse>(responseHandler));
        }

        public async Task<ArtistResponse> ArtistGetAsync(string artistid, string provider_instance_id_or_domain)
        {
            return await SendAsync<ArtistResponse>(c, IdAndProvider(Commands.ArtistGet, artistid, provider_instance_id_or_domain));
        }

        public void ArtistsGet(Action<ArtistsResponse> responseHandler)
        {
            c.Send<ArtistsResponse>(JustCommand(Commands.ArtistsGet), Deserialise<ArtistsResponse>(responseHandler));
        }

        public async Task<ArtistsResponse> ArtistsGetAsync()
        {
            return await SendAsync<ArtistsResponse>(c, JustCommand(Commands.ArtistsGet));
        }

        public void ArtistAlbums(string artistid, string provider_instance_id_or_domain, Action<AlbumsResponse> responseHandler)
        {
            c.Send<AlbumsResponse>(IdAndProvider(Commands.ArtistAlbums, artistid, provider_instance_id_or_domain), Deserialise<AlbumsResponse>(responseHandler));
        }

        public async Task<AlbumsResponse> ArtistAlbumsAsync(string artistid, string provider_instance_id_or_domain)
        {
            return await SendAsync<AlbumsResponse>(c, IdAndProvider(Commands.ArtistAlbums, artistid, provider_instance_id_or_domain));
        }

        public void ArtistCount(Action<CountResponse> responseHandler)
        {
            var m = new Message(Commands.ArtistsCount)
            {
                args = new Hashtable
                {
                    { "favorite_only", "false" },
                    { "album_artists_only", "true" }
                }
            };
            c.Send<CountResponse>(m, Deserialise<CountResponse>(responseHandler));
        }

        public async Task<CountResponse> ArtistCountAsync()
        {
            var m = new Message(Commands.ArtistsCount)
            {
                args = new Hashtable
                {
                    { "favorite_only", "false" },
                    { "album_artists_only", "true" }
                }
            };
            return await SendAsync<CountResponse>(c, m);
        }
    }
}
