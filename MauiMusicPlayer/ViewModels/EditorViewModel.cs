namespace MauiMusicPlayer.ViewModels;

public partial class EditorViewModel : ObservableObject
{
    private IMessageService _messageService;

    public EditorViewModel(IMessageService messageService)
    {
        _messageService = messageService;
    }

    private TagLib.File? CurrentTagLibFile { get; set; }

    [ObservableProperty] private string pathToCurrentFile = "none";

    [ObservableProperty] private string title = string.Empty;
    [ObservableProperty] private string album = string.Empty;
    [ObservableProperty] private string subtitle = string.Empty;
    [ObservableProperty] private string conductor = string.Empty;
    [ObservableProperty] private string comment = string.Empty;
    [ObservableProperty] private string performers = string.Empty;
    [ObservableProperty] private string albumArtists = string.Empty;
    [ObservableProperty] private string composers = string.Empty;
    [ObservableProperty] private string genres = string.Empty;
    [ObservableProperty] private uint year = 0;

    [RelayCommand]
    public async Task SelectFile()
    {
        var options = new PickOptions
        {
            FileTypes = Helper.DefaultWinUIFileTypes
        };
        var file = await FilePicker.Default.PickAsync(options);
        if (file == null) return;

        CurrentTagLibFile = TagLib.File.Create(file.FullPath);
        PathToCurrentFile = file.FullPath;

        Title = CurrentTagLibFile.Tag.Title;
        Album = CurrentTagLibFile.Tag.Album;
        Subtitle = CurrentTagLibFile.Tag.Subtitle;
        Conductor = CurrentTagLibFile.Tag.Conductor;
        Comment = CurrentTagLibFile.Tag.Comment;
        
        Performers = string.Join(";", CurrentTagLibFile.Tag.Performers);
        AlbumArtists = string.Join(";", CurrentTagLibFile.Tag.AlbumArtists);
        Composers = string.Join(";", CurrentTagLibFile.Tag.Composers);
        Genres = string.Join(";", CurrentTagLibFile.Tag.Genres);
        Year = CurrentTagLibFile.Tag.Year;
    }

    [RelayCommand]
    private async Task SaveChanges()
    {
        if (CurrentTagLibFile == null)
        {
            await _messageService.NotificationMessage("Not specified", "Can't save changes (file is not specified)", "Ok");
            return;
        }

        if (!await _messageService.AgreementMessageIsTrue("Changing metadata", "Do you want to save changes?",
            "Yes", "No")) return;

        CurrentTagLibFile.Tag.Title = Title;
        CurrentTagLibFile.Tag.Album = Album;
        CurrentTagLibFile.Tag.Subtitle = Subtitle;
        CurrentTagLibFile.Tag.Conductor = Conductor;
        CurrentTagLibFile.Tag.Comment = Comment;
        CurrentTagLibFile.Tag.Performers = Performers.Split(';');
        CurrentTagLibFile.Tag.AlbumArtists = AlbumArtists.Split(';');
        CurrentTagLibFile.Tag.Composers = Composers.Split(';');
        CurrentTagLibFile.Tag.Genres = Genres.Split(';');
        CurrentTagLibFile.Tag.Year = Year;

        try
        {
            CurrentTagLibFile.Save(); 
            await _messageService.NotificationMessage("Success", "Metadata was successfully changed", "Ok");
        }
        catch (Exception)
        {
            await _messageService.NotificationMessage("Failed", "File is opened by other thread and can't be saved", "Ok");
        }

    }

    [RelayCommand]
    private async Task CloseAndClearWithoutChanging()
    {
        if (CurrentTagLibFile == null) return;

        if (!await _messageService.AgreementMessageIsTrue("Close file", 
            "Do you want to close file? Unsaved metadata will be lost", "Yes", "No")) return;

        if (CurrentTagLibFile != null)
        {
            CurrentTagLibFile?.Dispose();

            CurrentTagLibFile = null;
            PathToCurrentFile = "none";
        }

        Title = string.Empty;
        Album = string.Empty;
        Subtitle = string.Empty;
        Conductor = string.Empty;
        Comment = string.Empty;
        Performers = string.Empty;
        AlbumArtists = string.Empty;
        Composers = string.Empty;
        Genres = string.Empty;
        Year = 0;
    }
}
