namespace MauiMusicPlayer.Views;

public partial class HomeView : ContentPage
{
    public HomeView(HomeViewModel vm, IAudioService audioService)
	{
		InitializeComponent();

		BindingContext = vm;
        audioService.InitMediaElement(MainAudioElement);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Window.MinimumWidth = Helper.AppMinWidth;
        Window.MinimumHeight = Helper.AppMinHeight;
    }
}