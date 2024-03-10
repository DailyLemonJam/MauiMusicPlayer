namespace MauiMusicPlayer.ViewModels;

public partial class PlaylistsViewModel : ObservableObject
{
    private IPlaylistsRepository _playlistRepository;
    private IAudioService _audioService;
    private IMessageService _messageService;

    public PlaylistsViewModel(IPlaylistsRepository playlistsRepository, IAudioService audioService, 
        IMessageService messageService)
    {
        _playlistRepository = playlistsRepository;
        _audioService = audioService;
        _messageService = messageService;

        Playlists = _playlistRepository.LoadPlaylists();
        SelectedPlaylist = null;
    }

    [ObservableProperty]
    private Playlist? selectedPlaylist;

    [ObservableProperty]
    private List<Playlist> playlists;

    [ObservableProperty]
    private List<Audio> currentPlaylistAudios = [];

    [RelayCommand]
    private async Task CreatePlaylist()
    {
        string? result = await _messageService.CreatePlaylistMessage();

        if (!await MeetsRequirements(result)) return;

        if (File.Exists(Path.Combine(AppFolder.PlaylistsPath, $"{result}.json")))
        {
            await _messageService.NotificationMessage("Already exists", "Playlist with this name already exists", "Ok");
            return;
        }

        var newPlaylist = new Playlist(result);

        _playlistRepository.SavePlaylist(newPlaylist);

        Playlists = _playlistRepository.LoadPlaylists();
        SelectedPlaylist = null;
        CurrentPlaylistAudios = [];
    }

    [RelayCommand]
    private void SelectPlaylist(Playlist playlist)
    {
        SelectedPlaylist = playlist;
        CurrentPlaylistAudios = playlist.Audios;
    }

    [RelayCommand]
    private async Task DeletePlaylist()
    {
        if (SelectedPlaylist == null)
        {
            await _messageService.NotificationMessage("Not selected", "Playlist wasn't selected", "Ok");
            return;
        }

        if (await _messageService.AgreementMessageIsTrue("Delete playlist", 
            "Do you want to delete selected playlist?", "Yes", "No"))
        {
            string path = Path.Combine(AppFolder.PlaylistsPath, $"{SelectedPlaylist.Name}.json");
            File.Delete(path);

            Playlists = _playlistRepository.LoadPlaylists();
            SelectedPlaylist = null;
            CurrentPlaylistAudios = [];
        }
    }

    [RelayCommand]
    private async Task RenamePlaylist()
    {
        if (SelectedPlaylist == null) 
        {
            await _messageService.NotificationMessage("Not selected", "Playlist wasn't selected", "Ok");
            return;
        }

        string? newName = await _messageService.RenamePlaylistMessage(SelectedPlaylist.Name);

        if (!await MeetsRequirements(newName)) return;

        if (File.Exists(Path.Combine(AppFolder.PlaylistsPath, $"{newName}.json")))
        {
            await _messageService.NotificationMessage("Already exists", "Playlist with this name already exists", "Ok");
            return;
        }

        string fileToDelete = Path.Combine(AppFolder.PlaylistsPath, $"{SelectedPlaylist.Name}.json");
        File.Delete(fileToDelete);

        SelectedPlaylist.Name = newName;
        _playlistRepository.SavePlaylist(SelectedPlaylist);

        Playlists = _playlistRepository.LoadPlaylists();
        SelectedPlaylist = null;
        CurrentPlaylistAudios = [];
    }

    [RelayCommand]
    private async Task AddAudioToSelectedPlaylist()
    {
        if (SelectedPlaylist == null)
        {
            await _messageService.NotificationMessage("Not selected", "Playlist wasn't selected", "Ok");
            return;
        }

        var options = new PickOptions
        {
            FileTypes = Helper.DefaultWinUIFileTypes
        };

        var pickedFiles = await FilePicker.Default.PickMultipleAsync(options);

        foreach (var file in pickedFiles)
        {
            var newAudio = new Audio(file.FullPath);
            SelectedPlaylist.Audios.Add(newAudio);
        }

        _playlistRepository.SavePlaylist(SelectedPlaylist);

        SelectedPlaylist = _playlistRepository.LoadPlaylistByName($"{SelectedPlaylist.Name}.json");
        CurrentPlaylistAudios = SelectedPlaylist.Audios;
    }

    public CollectionView PlaylistCollectionView;

    [RelayCommand]
    private void SelectAudioInPlaylist(Audio audio)
    {
        _audioService.SetAudio(audio, CurrentPlaylistAudios, PlaylistCollectionView);
    }

    private async Task<bool> MeetsRequirements(string? str)
    {
        if (str == null) return false;
        if (str.Length == 0)
        {
            await _messageService.NotificationMessage("Invalid input", "Length can't be zero", "Ok");
            return false;
        }
        if (char.IsWhiteSpace(str[0]) || char.IsWhiteSpace(str[^1]))
        {
            await _messageService.NotificationMessage("Invalid input", "Can't start or end with white space", "Ok");
            return false;
        }

        return true;
    }
}
