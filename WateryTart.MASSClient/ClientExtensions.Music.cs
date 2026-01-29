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

        public void TrackCount(Action<CountResponse> responseHandler)
        {
            c.Send<CountResponse>(JustId("music/tracks/count", "false", "favourite_only"), Deserialise<CountResponse>(responseHandler));
        }

        public void PlaylistGet(string playlistId, string provider_instance_id_or_domain, Action<PlaylistResponse> responseHandler)
        {
            c.Send<PlaylistResponse>(IdAndProvider(Commands.PlaylistGet, playlistId, provider_instance_id_or_domain), Deserialise<PlaylistResponse>(responseHandler));
        }

        public void PlaylistTracksGet(string playlistId, string provider_instance_id_or_domain, Action<TracksResponse> responseHandler)
        {
            c.Send<TracksResponse>(IdAndProvider(Commands.PlaylistTracksGet, playlistId, provider_instance_id_or_domain), Deserialise<TracksResponse>(responseHandler));
        }

        public void MusicAlbumsLibraryItems(Action<AlbumsResponse> responseHandler)
        {
            c.Send<AlbumsResponse>(JustCommand(Commands.MusicAlbumLibraryItems), Deserialise<AlbumsResponse>(responseHandler));
        }

        public void MusicAlbumGet(string id, string provider_instance_id_or_domain, Action<AlbumResponse> responseHandler)
        {
            c.Send<AlbumResponse>(IdAndProvider(Commands.MusicAlbumGet, id, provider_instance_id_or_domain), Deserialise<AlbumResponse>(responseHandler));
        }

        public void MusicAlbumTracks(string id, string provider_instance_id_or_domain, Action<TracksResponse> responseHandler)
        {
            c.Send<TracksResponse>(IdAndProvider(Commands.MusicAlbumTracks, id, provider_instance_id_or_domain), Deserialise<TracksResponse>(responseHandler));
        }
        
        public void MusicAlbumTracks(string id, Action<TracksResponse> responseHandler)
        {
            MusicAlbumTracks(c, id, "library", responseHandler);
        }

        public void MusicRecommendations(Action<RecommendationResponse> responseHandler)
        {
            c.Send<RecommendationResponse>(JustCommand(Commands.MusicRecommendations), Deserialise<RecommendationResponse>(responseHandler));
        }
    }
}