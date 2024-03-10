namespace MauiMusicPlayer.Views;

public partial class LibraryView : ContentPage
{
	public LibraryView(LibraryViewModel vm, IAudioService audioService)
	{
		InitializeComponent();

		BindingContext = vm;
		vm.LibraryCollectionView = libraryAudioCV;

        // Panel controls
        //audioService.InitMediaElement(MainAudioElement);
        audioService.InitTimeSlider(TimeSlider);
        audioService.InitVolumeSlider(VolumeSlider);
		audioService.InitLabels(CurrentTitle, CurrentPerformers, CurrentTime, TotalTime);
		audioService.InitButtons(PreviousButton, PlayButton, NextButton, RepeatButton, SettingsButton);
	}
}