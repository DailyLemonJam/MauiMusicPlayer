namespace MauiMusicPlayer.Views;

public partial class PlaylistsView : ContentPage
{
	public PlaylistsView(PlaylistsViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
		vm.PlaylistCollectionView = PlaylistCollectionView;
	}
}