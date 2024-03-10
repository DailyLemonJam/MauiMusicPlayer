namespace MauiMusicPlayer.Common;

public static class AppFile
{
    private static readonly string JSONExtension = ".json";

    // JSON file that contains user added folders with audio
    public static readonly string AddedFolders = Path.Combine(
        AppFolder.DataPath, $"AddedFolders{JSONExtension}");

}
