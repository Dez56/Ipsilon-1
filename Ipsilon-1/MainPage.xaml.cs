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
            var Cliente = new HttpClient();
            var url = DeviceInfo.Platform == DevicePlatform.Android
                ? "http://10.0.2.2:5015"
                : "http://localhost:5015";

            var resp = await Cliente.GetAsync($"{url}/WeatherForecast");

            var dato = await resp.Content.ReadAsStringAsync();

            helo.Text = dato;
        }
    }

}
