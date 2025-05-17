using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using Ipsilon_1.fleshy;
using Ipsilon_1.Views;

namespace Ipsilon_1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnSleep()
        {
            // Evita errores si el paquete es nulo
            if (Vars_Globales.pask != null && Vars_Globales.pask.Estado == 0)
            {
                try
                {
                    Task.Run(async () =>
                    {
                        if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                        {
                            Vars_Globales.pask.Estado = 2;
                            string url = $"{Vars_Globales.Uerel}/Paquetes/{Vars_Globales.pask.Id}";
                            using var client = new HttpClient();
                            var json = JsonConvert.SerializeObject(Vars_Globales.pask);
                            var content = new StringContent(json, Encoding.UTF8, "application/json");
                            await client.PutAsync(url, content);
                            Vars_Globales.pask = null;
                        }
                        else
                        {
                            // Puedes guardar un estado local si no hay internet
                        }
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error en OnSleep: {ex.Message}");
                }
            }
        }

    }
}
