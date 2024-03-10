namespace MauiMusicPlayer.ViewModels;

public partial class LibraryViewModel : ObservableObject
{
    private readonly ILibraryRepository _repository;
    private readonly IAudioService _audioService;
    private readonly IMessageService _messageService;

    public LibraryViewModel(ILibraryRepository repository, IAudioService audioService, IMessageService messageService)
    {
        _repository = repository;
        _audioService = audioService;
        _messageService = messageService;

        LibraryAudios = _repository.LoadLibraryAudios();
        AddedFolders = _repository.LoadAddedFolders();
    }

    [ObservableProperty]
    private List<Audio> libraryAudios;

    [ObservableProperty]
    private List<string> addedFolders;

    [RelayCommand]
    private async Task AddFolder()
    {
        var result = await FolderPicker.Default.PickAsync(default);

        if (result.IsSuccessful)
        {
            string newPath = result.Folder.Path;

            if (AddedFolders.Contains(newPath)) return;

            AddedFolders.Add(result.Folder.Path);
            _repository.SaveAddedFolders(AddedFolders);
            LibraryAudios = _repository.LoadLibraryAudios();
        }
    }

    [RelayCommand]
    private void RefreshLibrary()
    {
        LibraryAudios = _repository.LoadLibraryAudios();

        if (LibraryAudios.Count > 0)
        {
            SelectNewAudio(LibraryAudios[0]);
            return;
        }

        _messageService.NotificationMessage("Empty", "Library is empty, try to add new folders", "Ok");
    }

    public CollectionView LibraryCollectionView;

    [RelayCommand]
    private void SelectNewAudio(Audio audio)
    {
        _audioService.SetAudio(audio, LibraryAudios, LibraryCollectionView);
    }
}
