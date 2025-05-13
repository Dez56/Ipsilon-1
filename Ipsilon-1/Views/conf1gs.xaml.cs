using Ipsilon_1.fleshy;

namespace Ipsilon_1.Views;

public partial class conf1gs : ContentPage
{
	public conf1gs()
	{
		InitializeComponent();

        if(Vars_Globales.Uerel != null)
        {
            entryDireccion.Text = Vars_Globales.Uerel;
        }
        else
        {
            entryDireccion.Text = "https://";
        }
    }
    private void GuardarDireccion_Clicked(object sender, EventArgs e)
    {
        string nuevaDireccion = entryDireccion.Text.Trim();

        if (!string.IsNullOrEmpty(nuevaDireccion))
        {
            Preferences.Set("direccion_servidor", nuevaDireccion);
            DisplayAlert("Guardado", "Dirección del servidor actualizada.", "OK");
        }
        else
        {
            DisplayAlert("Error", "Ingresa una dirección válida.", "OK");
        }
    }
}