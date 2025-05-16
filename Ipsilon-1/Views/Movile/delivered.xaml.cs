using Newtonsoft.Json;
using Ipsilon_1.Models;
using Ipsilon_1.fleshy;
using System.Net.Http;

namespace Ipsilon_1.Views.Movile;

public partial class delivered : ContentPage
{
	public delivered(int ods)
	{
		InitializeComponent();
        tla0.Text = ods.ToString();
    }

    protected override async void OnAppearing()
    {
        var httpClient = new HttpClient();
        var apiUrl = $"{Vars_Globales.Uerel}/Paquetes/{tla0.Text}";

        try
        {
            var response = await httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var paquete = JsonConvert.DeserializeObject<Paquete>(jsonResponse);

                if (paquete != null)
                {
                    tla2.Text = paquete.Codigo;
                    tla3.Text = paquete.EstadoDescripcion;
                    tla4.Text = paquete.HorSal.ToString("HH:mm");
                    tla5.Text = paquete.HorEnt?.ToString("HH:mm") ?? "No entregado";
                    tla6.Text = paquete.link;

                    var apiUrl2 = $"{Vars_Globales.Uerel}/Usuarios/{paquete.Repártidor}";
                    var response2 = await httpClient.GetAsync(apiUrl2);
                    if (response2.IsSuccessStatusCode)
                    {
                        var jsonResponse2 = await response2.Content.ReadAsStringAsync();
                        var iusers = JsonConvert.DeserializeObject<Usuario>(jsonResponse2);
                        if (iusers != null)
                        {
                            tla1.Text = iusers.Nombre;
                        }
                        else
                        {
                            await DisplayAlert("Error", "No se pudo leer al repartidor", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se encontró el repartidor", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo leer el paquete", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "No se encontró el paquete", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Excepción", ex.Message, "OK");
        }
    }


    private async void open(object sender, EventArgs e)
    {
        await Launcher.Default.OpenAsync(new Uri(tla6.Text));
    }
}