namespace Ipsilon_1.Views;

public partial class NewPage2 : ContentPage
{
	public NewPage2(string name)
	{
		InitializeComponent();
		helo.Text = name;
    }

	private async void OnNavigateButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NewPage1());
    }
}