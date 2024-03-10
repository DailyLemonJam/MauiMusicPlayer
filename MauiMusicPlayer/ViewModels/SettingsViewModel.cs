namespace MauiMusicPlayer.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly IMessageService _messageService;
    private readonly LibraryViewModel _libraryViewModel;

    public SettingsViewModel(IMessageService messageService, LibraryViewModel libraryViewModel)
    {
        _messageService = messageService;
        _libraryViewModel = libraryViewModel;
    }

    [RelayCommand]
    private async Task Reset()
    {
        if (!await _messageService.AgreementMessageIsTrue("Reset", 
            "Do you want to reset? Library and playlists will be removed, application will be closed.",
            "Yes", "No"))
        {
            return;
        }

        var dataFolder = new DirectoryInfo(AppFolder.DataPath);

        foreach (var file in dataFolder.GetFiles())
        {
            file.Delete();
        }

        var playlistsFolder = new DirectoryInfo(AppFolder.PlaylistsPath);
        foreach (var playlist in playlistsFolder.GetFiles())
        {
            playlist.Delete();
        }

        await _messageService.NotificationMessage("Removed", "All files have been removed", "Ok");

        Environment.Exit(0);
        //Process.Start(AppDomain.CurrentDomain.BaseDirectory, "/restart" + Environment.ProcessId);
    }

    [RelayCommand]
    private void OpenTagLibGitHub()
    {
        Launcher.OpenAsync("https://github.com/mono/taglib-sharp").Wait();
    }
}
