using WateryTart.Service.MassClient.Models;

namespace WateryTart.Service.MassClient.Events;

public class PlayerEventResponse : BaseEventResponse
{
    public Player data { get; set; }
}


public class MediaItemEventResponse : BaseEventResponse
{
    public MediaItemEventItem data { get; set; }
}


public class MediaItemEventItem 
{
    public string uri { get; set; }
    public string media_type { get; set; }
    public string name { get; set; }
    public int duration { get; set; }
    public int seconds_played { get; set; }
    public bool fully_played { get; set; }
    public bool is_playing { get; set; }
    public object mbid { get; set; }
    public string artist { get; set; }
    public List<string> artists { get; set; }
    public List<object> artist_mbids { get; set; }
    public string album { get; set; }
    public object album_mbid { get; set; }
    public string album_artist { get; set; }
    public List<object> album_artist_mbids { get; set; }
    public string image_url { get; set; }
    public string version { get; set; }
    public string userid { get; set; }
}