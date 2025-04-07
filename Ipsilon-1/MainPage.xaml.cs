using Ipsilon_1.Views;


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
            var url = DeviceInfo.Platform == DevicePlatform.Android
                ? "https://10.0.2.2:7169"
                : "https://localhost:7169";

            var resp = await Cliente.GetAsync($"{url}/WeatherForecast");

            var dato = await resp.Content.ReadAsStringAsync();

            helo.Text = dato;
        }

        private async void OnNavigate2Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewPage1());
        }
    }

}
