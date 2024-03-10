namespace MauiMusicPlayer.Common;

public static class AppFolder
{
    // Main folder name (root for other folders)
    private static readonly string Main = "MauiMusicPlayer";

    // App data folder name (for app JSONs)
    private static readonly string Data = "Data";

    // Folder with playlists (1 JSON == 1 Playlist)
    private static readonly string Playlists = "Playlists";

    // -------------------- Paths --------------------
    public static readonly string MainPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Main);

    public static readonly string DataPath = Path.Combine(MainPath, Data);

    public static readonly string PlaylistsPath = Path.Combine(MainPath, Playlists);
}
