namespace Ipsilon_1.Views;

public partial class NewPage1 : ContentPage
{
	public NewPage1()
	{
		InitializeComponent();
	}

	private async void OnNavigateButtonClicked(object sender, EventArgs e)
	{
        var nombre = NombreEntry.Text; // Asumiendo que tienes un Entry para el nombre
        var contrasena = ContrasenaEntry.Text; // Asumiendo que tienes un Entry para la contraseña

        var Cliente = new HttpClient();
        var url = DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2:5015"
            : "http://localhost:5015";

        var response = await Cliente.GetAsync($"{url}/Usuarios/ByName?nombre={nombre}&contrasena={contrasena}");

        if (response.IsSuccessStatusCode)
        {
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Navigation.PushAsync(new MovileQr(nombre));
            }
            else
            {
                await Navigation.PushAsync(new HubPlatform());
            }
        }
        else
        {
            await Navigation.PushAsync(new HubPlatform());
            helo.Text = "Usuario no encontrado o contraseña incorrecta";
        }
    }
}