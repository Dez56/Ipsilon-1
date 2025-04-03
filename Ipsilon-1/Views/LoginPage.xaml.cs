using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Ipsilon_1A.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var username = UsernameEntry.Text;
            var password = PasswordEntry.Text;

            var loginModel = new { Username = username, Password = password };
            var json = JsonSerializer.Serialize(loginModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            var url = DeviceInfo.Platform == DevicePlatform.Android
                ? "http://10.0.2.2:5015"
                : "http://localhost:5015";

            var response = await client.PostAsync(($"{url}/auth/login"), content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var token = JsonSerializer.Deserialize<TokenResponse>(responseContent)?.Token;

                // Guardar el token para futuras solicitudes
                await SecureStorage.SetAsync("auth_token", token);

                // Navegar a la página principal
                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                await DisplayAlert("Error", "Login failed", "OK");
            }
        }
    }

    public class TokenResponse
    {
        public required string Token { get; set; }
    }
}