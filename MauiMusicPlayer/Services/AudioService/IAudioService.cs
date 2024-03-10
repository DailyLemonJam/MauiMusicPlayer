namespace MauiMusicPlayer.Services.AudioService;

public interface IAudioService
{
    // Main
    void InitMediaElement(MediaElement mediaElement);
    void InitTimeSlider(Slider timeSlider);
    void InitVolumeSlider(Slider volumeSlider);
    void InitLabels(Label currentTitle, Label currentPerformers, Label currentTime, Label totalTime);
    void InitButtons(ImageButton previous, ImageButton play, ImageButton next, 
        ImageButton repeat, ImageButton settings);

    // Control
    void SetAudio(Audio audio, List<Audio> audioList, CollectionView currentCollectionView);
}
