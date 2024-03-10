namespace MauiMusicPlayer.Services.MessageService;

public interface IMessageService
{
    Task<string> CreatePlaylistMessage();

    Task<string> RenamePlaylistMessage(string initialName);

    Task NotificationMessage(string title, string message, string cancel);

    Task<bool> AgreementMessageIsTrue(string title, string question, string accept, string cancel);
}
