namespace MauiMusicPlayer.Common;

internal static class Helper
{
    public static readonly int AppMinWidth = 970; // Shell width + LibraryView width
    public static readonly int AppMinHeight = 250;

    public static readonly FilePickerFileType DefaultWinUIFileTypes = new(
        new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.WinUI, new[] { ".mp3", ".wav" } },
        });

    public static string BuildStringDuration(TimeSpan duration)
    {
        var sb = new StringBuilder();

        if (duration.Hours < 10) sb.Append('0');

        sb.Append(duration.Hours);
        sb.Append(':');

        if (duration.Minutes < 10) sb.Append('0');

        sb.Append(duration.Minutes);
        sb.Append(':');

        if (duration.Seconds < 10) sb.Append('0');

        sb.Append(duration.Seconds);

        return sb.ToString();
    }
}
