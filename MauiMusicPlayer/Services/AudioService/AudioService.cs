namespace MauiMusicPlayer.Services.AudioService;

public class AudioService(IMessageService messageService) : IAudioService
{
    private readonly IMessageService _messageService = messageService;

    // MediaElement should be set from HomeView directly only once via SetMediaElement method
    // (Because initially it must be in XAML page and can't be created from the code)
    private MediaElement MainMediaElement { get; set; }
    private Slider TimeSlider { get; set; }
    private Slider VolumeSlider { get; set; }
    private Label CurrentTitle { get; set; }
    private Label CurrentPerformers { get; set; }
    private Label CurrentTime { get; set; }
    private Label TotalTime { get; set; }
    private ImageButton Previous { get; set; }
    private ImageButton Play { get; set; }
    private ImageButton Next { get; set; }
    private ImageButton Repeat { get; set; }
    private ImageButton Settings { get; set; }

    // Inside info
    private Audio CurrentAudio { get; set; }
    private List<Audio> CurrentAudioList { get; set; }
    private CollectionView CurrentCollectionView { get; set; } // for visual selection

    public void InitButtons(ImageButton previous, ImageButton play, ImageButton next, 
        ImageButton repeat, ImageButton settings)
    {
        Previous = previous;
        Play = play;
        Next = next;
        Repeat = repeat;
        Settings = settings;

        Previous.Clicked += (s, e) => SetAudio(GetPreviousAudio(), CurrentAudioList, CurrentCollectionView);
        Play.Clicked += (s, e) =>
        {
            // TODO Check all states?
            var state = MainMediaElement.CurrentState;

            switch (state)
            {
                case MediaElementState.None:
                    _messageService.NotificationMessage("Not selected", "Audio wasn't selected", "Ok");
                    break;
                case MediaElementState.Opening:
                    break;
                case MediaElementState.Buffering:
                    break;
                case MediaElementState.Playing:
                    {
                        MainMediaElement.Pause();
                        Play.Source = "play_icon.png";
                        break;
                    }
                case MediaElementState.Paused:
                    {
                        MainMediaElement.Play();
                        Play.Source = "pause_icon.png";
                        break;
                    }
                case MediaElementState.Stopped:
                    break;
                case MediaElementState.Failed:
                    break;
            }
        };
        Next.Clicked += (s, e) => SetAudio(GetNextAudio(), CurrentAudioList, CurrentCollectionView);
        Repeat.Clicked += (s, e) =>
        {
            Debug.WriteLine(Brush.Black.ToString());

            if (!MainMediaElement.ShouldLoopPlayback)
                Repeat.BorderColor = Brush.Black.Color;
            else Repeat.BorderColor = Brush.WhiteSmoke.Color;

            MainMediaElement.ShouldLoopPlayback = !MainMediaElement.ShouldLoopPlayback;
        };
        Settings.Clicked += (s, e) =>
        {
            Debug.WriteLine("Clicked Settings button");
        };
    }

    public void InitLabels(Label currentTitle, Label currentPerformers, Label currentTime, Label totalTime)
    {
        CurrentTitle = currentTitle;
        CurrentPerformers = currentPerformers;
        CurrentTime = currentTime;
        TotalTime = totalTime;
    }

    public void InitMediaElement(MediaElement mediaElement)
    {
        if (MainMediaElement == null)
            MainMediaElement = mediaElement;
        else Debug.Assert(false, "Trying to set MediaElement again");

        MainMediaElement.PositionChanged += (s, e) =>
        {
            double newValue = e.Position.TotalSeconds / CurrentAudio.Duration.TotalSeconds;
            TimeSlider.Value = Math.Clamp(newValue, 0.0, 1.0);

            CurrentTime.Text = Helper.BuildStringDuration(MainMediaElement.Position);
        };

        MainMediaElement.MediaEnded += (_, e) =>
        {
            // Calling SetAudio directly without dispatcher causes issues
            MainMediaElement.Dispatcher.Dispatch(() =>
            {
                SetAudio(GetNextAudio(), CurrentAudioList, CurrentCollectionView);
            });
        };
    }

    public void InitTimeSlider(Slider timeSlider)
    {
        if (TimeSlider == null)
            TimeSlider = timeSlider;
        else Debug.Assert(false, "Trying to set TimeSlider again");

        TimeSlider.ValueChanged += (_, e) =>
        {
            var newPosition = MainMediaElement.Duration.Divide(1 / e.NewValue);
            MainMediaElement.SeekTo(newPosition).Wait();
        };
    }

    public void InitVolumeSlider(Slider volumeSlider)
    {
        if (VolumeSlider == null)
            VolumeSlider = volumeSlider;
        else Debug.Assert(false, "Trying to set VolumeSlider again");

        VolumeSlider.ValueChanged += (_, e) =>
        {
            MainMediaElement.Volume = e.NewValue;
        };

        VolumeSlider.Value = 0.1;
    }

    public void SetAudio(Audio audio, List<Audio> audioList, CollectionView currentCollectionView)
    {
        if (audioList.Count == 0)
        {
            _messageService.NotificationMessage("Empty", "Library is empty", "Ok");
            return;
        }

        if (Play.Source.ToString() != "File: pause_icon.png")
            Play.Source = "pause_icon.png";

        CurrentAudio = audio;
        CurrentAudioList = audioList;
        CurrentCollectionView = currentCollectionView;

        if (!File.Exists(CurrentAudio.Path))
        {
            _messageService.NotificationMessage("Not found", "File is missing", "Ok");
            return;
        }

        CurrentTitle.Text = CurrentAudio.Title;
        CurrentPerformers.Text = CurrentAudio.Performers;
        TotalTime.Text = CurrentAudio.RoundedDuration;

        CurrentCollectionView.SelectedItem = CurrentAudio;

        MainMediaElement.Source = MediaSource.FromUri(CurrentAudio.Path);
        MainMediaElement.Play();
    }

    private Audio GetNextAudio()
    {
        // TODO Proper indexing (mb)

        var currentAudioIndex = CurrentAudioList.FindIndex(
                    e => e.GetHashCode() == CurrentAudio.GetHashCode());

        if (currentAudioIndex == -1)
        {
            Debug.Assert(false, "How did that even happen??");
            return CurrentAudio;
        }

        if (currentAudioIndex == CurrentAudioList.Count - 1)
            currentAudioIndex = 0;
        else currentAudioIndex++;

        return CurrentAudioList[currentAudioIndex];
    }

    private Audio GetPreviousAudio()
    {
        var currentAudioIndex = CurrentAudioList.FindIndex(
                    e => e.GetHashCode() == CurrentAudio.GetHashCode());

        if (currentAudioIndex == -1)
        {
            Debug.Assert(false, "How did that even happen??");
            return CurrentAudio;
        }

        if (currentAudioIndex == 0)
            currentAudioIndex = CurrentAudioList.Count - 1;
        else currentAudioIndex--;

        return CurrentAudioList[currentAudioIndex];
    }
}
