using Ipsilon_1.Views.movint;

namespace Ipsilon_1.Views;

public partial class HubPlatform : ContentPage
{
	public HubPlatform()
	{
		InitializeComponent();
	}
	
	private void red1(object sender, EventArgs e)
    {
        // Navegar a la página de red1
        Navigation.PushAsync(new Agregar());
    }
    private void red2(object sender, EventArgs e)
    {
        // Navegar a la página de red1
        Navigation.PushAsync(new Consultas());
    }
    private void red3(object sender, EventArgs e)
    {
        // Navegar a la página de red1
        Navigation.PushAsync(new Editar());
    }
}