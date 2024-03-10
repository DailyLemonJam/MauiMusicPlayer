using System.Text;

namespace MauiMusicPlayer.Models;

// Represents an audio file with path to it
public class Audio
{
    public Audio(string path)
    {
        Path = path;

        var tl = TagLib.File.Create(path);

        Title = tl.Tag.Title;
        Performers = string.Join(";", tl.Tag.Performers);
        Album = tl.Tag.Album;
        Duration = tl.Properties.Duration;

        RoundedDuration = Helper.BuildStringDuration(Duration);

        tl.Dispose();
    }

    public string Path { get; set; }
    public string Title { get; set; }
    public string Performers { get; set; }
    public string Album { get; set; }
    public TimeSpan Duration { get; set; }
    public string RoundedDuration { get; set; }

    public override int GetHashCode()
    {
        // relatively random
        return Path.GetHashCode() / 3 + Duration.GetHashCode() / 5 + RoundedDuration.GetHashCode() / 7;
    }
}
