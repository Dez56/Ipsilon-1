using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Ipsilon_1A.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnSendDataButtonClicked(object sender, EventArgs e)
        {
            var data = new { /* datos a enviar */ };
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            var token = await SecureStorage.GetAsync("auth_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync("https://yourapiurl/api/yourendpoint", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Success", "Data sent successfully", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Failed to send data", "OK");
            }
        }
    }
}