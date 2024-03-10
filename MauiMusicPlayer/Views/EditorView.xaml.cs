namespace MauiMusicPlayer.Views;

public partial class EditorView : ContentPage
{
	public EditorView(EditorViewModel vm)
	{
		InitializeComponent();
		
		BindingContext = vm;
	}

    private void YearValidation(object sender, TextChangedEventArgs e)
    {
		if (!int.TryParse(e.NewTextValue, out _) || e.NewTextValue.Contains(' '))
		{
            yearField.Text = "0";
		}
    }
}