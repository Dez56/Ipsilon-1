namespace Ipsilon_1.Views.Movile;
using Ipsilon_1.Views.Movile;
using Ipsilon_1.Models;
using Ipsilon_1.fleshy;
using Ipsilon_1.Views;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Microsoft.Maui.Controls;

public partial class DelivMood : ContentPage
{

    double translationX = 0;

    public DelivMood(string ido)
    {
        InitializeComponent();
        Codicia.Text = ido;
        Avaricia.Text = Vars_Globales.Nii;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await getshit();

        if (Vars_Globales.pask != null)
        {
            Mentiras.Text = Vars_Globales.pask.HorSal.ToString("HH:mm");
        }
        else
        {
            await DisplayAlert("Error", "No se obtuvo la información del paquete", "OK");
            await Navigation.PopAsync();
        }
    }

    public async Task getshit()
    {
        var httpClient = new HttpClient();
        var apiUrl = $"{Vars_Globales.Uerel}/Paquetes/BuscarPorCodigo/{Codicia.Text}";
        var response = await httpClient.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var paquete = JsonConvert.DeserializeObject<Paquete>(jsonResponse);
            if (paquete != null)
            {
                Vars_Globales.pask = paquete;
            }
            else {
                await DisplayAlert("Error", "Error insesperado", "OK");
                await Navigation.PopAsync();
            }
        } else
        {
            await DisplayAlert("Error", "No se encontro el servidor", "OK");
            await Navigation.PopAsync();
        }
    }

    public async Task BullshitGo (int stat)
    {
        //este no deberia usarse nunca, es imposible que se llegue a usar, esta solo para que no salgan advertencias de que nulos y eso
        if (Vars_Globales.pask == null)
        {
            await DisplayAlert("Error", "El paquete no está inicializado", "OK");
            return;
        }

        var paquete = new Paquete
        {
            Id = Vars_Globales.pask.Id,
            Repartidor = Vars_Globales.UeserID,
            Codigo = Vars_Globales.pask.Codigo,
            HorSal = Vars_Globales.pask.HorSal,
            HorEnt = stat == 1 ? DateTime.Now : null,
            Estado = stat,
            link = Vars_Globales.pask.link
        };
        string url = $"{Vars_Globales.Uerel}Paquetes/{paquete.Id}";
        using (HttpClient client = new HttpClient())
        {
            var json = JsonConvert.SerializeObject(paquete);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(url, content);
            Vars_Globales.pask = null;
            await DisplayAlert("Entregado", $"EL resultado de la operacion fue{response.IsSuccessStatusCode}", "OK");
        }
    }

    private void OnSliderPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Running:
                translationX += e.TotalX;
                translationX = Math.Max(-120, Math.Min(120, translationX));
                SliderThumb.TranslationX = translationX;
                break;

            case GestureStatus.Completed:

                //derecha
                if (translationX > 100)
                {
                    
                    DisplayAlert("Acción", "Deslizaste a la DERECHA", "OK");
                }
                //izquierda
                else if (translationX < -100)
                {
                    DisplayAlert("Acción", "Deslizaste a la IZQUIERDA", "OK");
                }

                //reseteo
                SliderThumb.TranslateTo(0, 0, 150, Easing.CubicOut);
                translationX = 0;
                break;
        }
    }


}