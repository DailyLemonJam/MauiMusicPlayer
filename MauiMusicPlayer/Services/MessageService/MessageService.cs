namespace MauiMusicPlayer.Services.MessageService;

public class MessageService : IMessageService
{
    private readonly Page MainPage;
    public MessageService()
    {
        if (Application.Current == null || Application.Current.MainPage == null)
        {
            throw new ApplicationException("Current application is null");
        }

        MainPage = Application.Current.MainPage;
    }

    public Task<bool> AgreementMessageIsTrue(string title, string question, string accept, string cancel)
    {
        return MainPage.DisplayAlert(title, question, accept, cancel);
    }

    public async Task<string> CreatePlaylistMessage()
    {
        string result = await MainPage.DisplayPromptAsync("Create playlist", string.Empty,
            "Create", "Back", "My New Playlist", 50, Keyboard.Text, string.Empty);

        return result;
    }

    public async Task NotificationMessage(string title, string message, string cancel)
    {
        await MainPage.DisplayAlert(title, message, cancel);
    }

    public async Task<string> RenamePlaylistMessage(string initialName)
    {
        string result = await MainPage.DisplayPromptAsync("Rename playlist", string.Empty,
            "Rename", "Back", "New Playlist Title", 50, Keyboard.Text, initialName);

        return result;
    }
}
