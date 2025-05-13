namespace Ipsilon_1.Views;

public partial class NewPage1 : ContentPage
{
    public NewPage1()
    {
        InitializeComponent();
    }

    private async void OnNavigateButtonClicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new MovileQr());

        /*var nombre = NombreEntry.Text;
        var contrasena = ContrasenaEntry.Text;

        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };

        var Cliente = new HttpClient(handler);
        var url = DeviceInfo.Platform == DevicePlatform.Android
            ? "https://10.0.2.2:7169"
            : "https://localhost:7169";

        var response = await Cliente.GetAsync($"{url}/Usuarios/ByName?nombre={nombre}&contrasena={contrasena}");

        if (response.IsSuccessStatusCode)
        {*/
        if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
        {
            await Navigation.PushAsync(new MovileQr());
        }
        else
        {
            await Navigation.PushAsync(new HubPlatform());
        }/*
        }
        else
        {
            await Navigation.PushAsync(new HubPlatform());
            helo.Text = "Usuario no encontrado o contraseña incorrecta";
        }*/
    }

    private async void gointoco(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new conf1gs());
    }
}