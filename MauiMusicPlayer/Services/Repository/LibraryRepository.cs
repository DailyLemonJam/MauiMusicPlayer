namespace MauiMusicPlayer.Services.Repository;

public class LibraryRepository(IJSONService jSONService) : ILibraryRepository
{
    private readonly IJSONService _JSONService = jSONService;

    public List<Audio> LoadLibraryAudios()
    {
        var addedFolders = LoadAddedFolders();

        var list = new List<Audio>();

        foreach (var folder in addedFolders)
        {
            var paths = Directory.EnumerateFiles(folder, "*.*", SearchOption.TopDirectoryOnly)
                .Where(e => e.EndsWith(".mp3") || e.EndsWith(".wav"));

            foreach (var path in paths)
            {
                list.Add(new Audio(path));
            }
        }

        return list;
    }

    public List<string> LoadAddedFolders()
    {
        if (File.Exists(AppFile.AddedFolders))
        {
            var folders = _JSONService.Load<List<string>>(AppFile.AddedFolders);

            return folders;
        }

        var emptyFolders = new List<string>();
        _JSONService.Save(emptyFolders, AppFile.AddedFolders);

        return _JSONService.Load<List<string>>(AppFile.AddedFolders);
    }

    public void SaveAddedFolders(List<string> addedFolders)
    {
        _JSONService.Save(addedFolders, AppFile.AddedFolders);
    }
}
