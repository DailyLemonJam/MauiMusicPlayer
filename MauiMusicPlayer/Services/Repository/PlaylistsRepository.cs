namespace MauiMusicPlayer.Services.Repository;

public class PlaylistsRepository(IJSONService jSONService) : IPlaylistsRepository
{
    private readonly IJSONService _JSONService = jSONService;

    public Playlist LoadPlaylistByName(string fullName)
    {
        string path = Path.Combine(AppFolder.PlaylistsPath, fullName);

        if (!File.Exists(path)) throw new FileNotFoundException();

        var playlist = _JSONService.Load<Playlist>(path);

        return playlist;
    }

    public List<Playlist> LoadPlaylists()
    {
        var pathsToFiles = Directory.EnumerateFiles(AppFolder.PlaylistsPath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(e => e.EndsWith(".json"));

        var playlists = new List<Playlist>();

        foreach (var path in pathsToFiles)
        {
            var loadedPlaylist = _JSONService.Load<Playlist>(path);

            playlists.Add(loadedPlaylist);
        }

        return playlists;
    }

    public void SavePlaylist(Playlist playlist)
    {
        _JSONService.Save(playlist, Path.Combine(AppFolder.PlaylistsPath, $"{playlist.Name}.json"));
    }
}
