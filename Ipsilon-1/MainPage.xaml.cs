using Ipsilon_1.Views;
using Ipsilon_1.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Ipsilon_1.fleshy;
using Ipsilon_1.Views.movint;
using Ipsilon_1.Views.Movile;


namespace Ipsilon_1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnNavigateButtonClicked(object sender, EventArgs e)
        {
            var nombre = NombreEntry.Text;
            var contrasena = ContrasenaEntry.Text;

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            var Cliente = new HttpClient(handler);
            var url = Vars_Globales.Uerel;

            var response = await Cliente.GetAsync($"{url}/Usuarios/ByName?nombre={nombre}&contrasena={contrasena}");
            

            if (response.IsSuccessStatusCode)
            {

                var json = await response.Content.ReadAsStringAsync();
                var usuario = JsonConvert.DeserializeObject<Usuario>(json);

                if (usuario != null) // Check if 'usuario' is not null
                {
                    Vars_Globales.UeserID = usuario.Id;
                    Vars_Globales.Nii = nombre;

                    if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        await Navigation.PushAsync(new MovileQr());
                    }
                    else
                    {
                        await Navigation.PushAsync(new Consultas());
                    }
                }
                else
                {
                    await DisplayAlert("Error", "El servidor no proceso bien la información, intente luego", "OK");
                }

            }
            else
            {
                await DisplayAlert("Error", "Usuario o contraseña incorrectos", "OK");
            }
            
        }

        private async void gointoco(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new conf1gs());
        }

        private async void redire(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new delivered());

        }
    }
}
