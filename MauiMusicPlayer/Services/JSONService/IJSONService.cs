namespace MauiMusicPlayer.Services.JSONService;

public interface IJSONService
{
    T Load<T>(string filePath);

    void Save<T>(T data, string filePath);
}
