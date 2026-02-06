namespace WateryTart.Service.MassClient.Models;

public class Search
{
    public List<Artist> artists { get; set; }
    public List<Album> albums { get; set; }
    public List<object> genres { get; set; }
    public List<Item> tracks { get; set; }
    public List<Playlist> playlists { get; set; }
    public List<object> radio { get; set; }
    public List<object> audiobooks { get; set; }
    public List<object> podcasts { get; set; }
}