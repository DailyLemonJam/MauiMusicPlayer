namespace MauiMusicPlayer.Services.Repository;

public interface ILibraryRepository
{
    List<Audio> LoadLibraryAudios();

    List<string> LoadAddedFolders();

    void SaveAddedFolders(List<string> folders);
}
