namespace MauiMusicPlayer.Services.Repository;

public interface IPlaylistsRepository
{
    public Playlist LoadPlaylistByName(string fullName);
    public List<Playlist> LoadPlaylists();
    public void SavePlaylist(Playlist playlist);
}
