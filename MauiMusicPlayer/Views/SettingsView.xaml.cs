namespace MauiMusicPlayer.Views;

public partial class SettingsView : ContentPage
{
	public SettingsView(SettingsViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}