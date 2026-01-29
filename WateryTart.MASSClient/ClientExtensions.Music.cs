using Newtonsoft.Json;
using System.Collections;
using WateryTart.MassClient.Messages;
using WateryTart.MassClient.Models;
using WateryTart.MassClient.Responses;

namespace WateryTart.MassClient;


public static partial class MassClientExtensions
{
    extension(IMassWsClient c)
    {

        /*   public static MessageBase PlaylistCount()
    {
        var m = new Message(Commands.ArtistsCount)
        {
            args = new Hashtable
            {
                { "favorite_only", "false" },
                { "album_artists_only", "true" }
            }
        };

        return m;
    }*/
        public void AlbumsCount(Action<CountResponse> responseHandler)
        {
            var m = new Message("music/albums/count")
            {
                args = new Hashtable
                {
                    { "favorite_only", "false" },
                    { "album_types", "[\"album\", \"single\", \"live\", \"soundtrack\", \"compilation\", \"ep\", \"unknown\"]" }
                }
            };

            c.Send<CountResponse>(m, Deserialise<CountResponse>(responseHandler));
        }

        public async Task<CountResponse> AlbumsCountAsync()
        {
            var m = new Message("music/albums/count")
            {
                args = new Hashtable
                {
                    { "favorite_only", "false" },
                    { "album_types", "[\"album\", \"single\", \"live\", \"soundtrack\", \"compilation\", \"ep\", \"unknown\"]" }
                }
            };

            return await SendAsync<CountResponse>(c, m);
        }

        public void TrackCount(Action<CountResponse> responseHandler)
        {
            c.Send<CountResponse>(JustId("music/tracks/count", "false", "favourite_only"), Deserialise<CountResponse>(responseHandler));
        }

        public async Task<CountResponse> TrackCountAsync()
        {
            return await SendAsync<CountResponse>(c, JustId("music/tracks/count", "false", "favourite_only"));
        }

        public void PlaylistGet(string playlistId, string provider_instance_id_or_domain, Action<PlaylistResponse> responseHandler)
        {
            c.Send<PlaylistResponse>(IdAndProvider(Commands.PlaylistGet, playlistId, provider_instance_id_or_domain), Deserialise<PlaylistResponse>(responseHandler));
        }

        public async Task<PlaylistResponse> PlaylistGetAsync(string playlistId, string provider_instance_id_or_domain)
        {
            return await SendAsync<PlaylistResponse>(c, IdAndProvider(Commands.PlaylistGet, playlistId, provider_instance_id_or_domain));
        }

        public void PlaylistTracksGet(string playlistId, string provider_instance_id_or_domain, Action<TracksResponse> responseHandler)
        {
            c.Send<TracksResponse>(IdAndProvider(Commands.PlaylistTracksGet, playlistId, provider_instance_id_or_domain), Deserialise<TracksResponse>(responseHandler));
        }

        public async Task<TracksResponse> PlaylistTracksGetAsync(string playlistId, string provider_instance_id_or_domain)
        {
            return await SendAsync<TracksResponse>(c, IdAndProvider(Commands.PlaylistTracksGet, playlistId, provider_instance_id_or_domain));
        }

        public void MusicAlbumsLibraryItems(Action<AlbumsResponse> responseHandler)
        {
            c.Send<AlbumsResponse>(JustCommand(Commands.MusicAlbumLibraryItems), Deserialise<AlbumsResponse>(responseHandler));
        }

        public async Task<AlbumsResponse> MusicAlbumsLibraryItemsAsync()
        {
            return await SendAsync<AlbumsResponse>(c, JustCommand(Commands.MusicAlbumLibraryItems));
        }

        public void MusicAlbumGet(string id, string provider_instance_id_or_domain, Action<AlbumResponse> responseHandler)
        {
            c.Send<AlbumResponse>(IdAndProvider(Commands.MusicAlbumGet, id, provider_instance_id_or_domain), Deserialise<AlbumResponse>(responseHandler));
        }

        public async Task<AlbumResponse> MusicAlbumGetAsync(string id, string provider_instance_id_or_domain)
        {
            return await SendAsync<AlbumResponse>(c, IdAndProvider(Commands.MusicAlbumGet, id, provider_instance_id_or_domain));
        }

        public void MusicAlbumTracks(string id, string provider_instance_id_or_domain, Action<TracksResponse> responseHandler)
        {
            c.Send<TracksResponse>(IdAndProvider(Commands.MusicAlbumTracks, id, provider_instance_id_or_domain), Deserialise<TracksResponse>(responseHandler));
        }

        public async Task<TracksResponse> MusicAlbumTracksAsync(string id, string provider_instance_id_or_domain)
        {
            return await SendAsync<TracksResponse>(c, IdAndProvider(Commands.MusicAlbumTracks, id, provider_instance_id_or_domain));
        }

        public void MusicRecommendations(Action<RecommendationResponse> responseHandler)
        {
            c.Send<RecommendationResponse>(JustCommand(Commands.MusicRecommendations), Deserialise<RecommendationResponse>(responseHandler));
        }

        public async Task<RecommendationResponse> MusicRecommendationsAsync()
        {
            return await SendAsync<RecommendationResponse>(c, JustCommand(Commands.MusicRecommendations));
        }
    }
}
