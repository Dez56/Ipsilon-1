using Ipsilon_1.Views;
using Ipsilon_1.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Ipsilon_1.fleshy;


namespace Ipsilon_1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            var Cliente = new HttpClient(handler);
            var url = Vars_Globales.Uerel;

            var resp = await Cliente.GetAsync($"{url}/WeatherForecast");

            var dato = await resp.Content.ReadAsStringAsync();

            helo.Text = dato;
        }

        private async void OnNavigate2Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewPage1());
        }


        private async void oh_no_abtn(object sender, EventArgs e)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            var Cliente = new HttpClient(handler);
            var url = Vars_Globales.Uerel;

            var resp = await Cliente.GetAsync($"{url}/Usuarios");

            var dato = await resp.Content.ReadAsStringAsync();

            helo.Text = dato;
        }

        //Usuarios Main metod

        private async void OnAgregarUsuarioClicked(object sender, EventArgs e)
        {
            var usuario = new Usuario
            {
                Nombre = NombreEntry.Text,
                Contrasena = ContrasenaEntry.Text
            };

            var resultado = await AgregarUsuarioAsync(usuario);
            await DisplayAlert("Se ha agregado un usuario", "Usuario agregado exitosamente", "OK");
            NombreEntry.Text = string.Empty;
            ContrasenaEntry.Text = string.Empty;
        }

        //Usuario util task
        private async Task<bool> AgregarUsuarioAsync(Usuario usuario)
        {
            string url = $"{Vars_Globales.Uerel}/Usuarios";
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(usuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);

                return response.IsSuccessStatusCode;
            }
        }
    }

}
