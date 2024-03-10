namespace MauiMusicPlayer.Services.JSONService;

public class JSONService : IJSONService
{
    public T Load<T>(string filePath)
    {
        string rawData = File.ReadAllText(filePath);

        T? data = JsonSerializer.Deserialize<T>(rawData);

        if (data == null) throw new Exception("Loaded data is null");

        return data;
    }

    public void Save<T>(T data, string filePath)
    {
        string serializedData = JsonSerializer.Serialize(data);
        
        File.WriteAllText(filePath, serializedData);
    }
}
