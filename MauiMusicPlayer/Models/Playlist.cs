namespace MauiMusicPlayer.Models;

// Represents list of audios
public class Playlist(string name)
{
    public string Name { get; set; } = name;

    public List<Audio> Audios { get; set; } = [];

    public override string ToString()
    {
        return $"Playlist name: {Name}, audio count: {Audios.Count}";
    }
}
