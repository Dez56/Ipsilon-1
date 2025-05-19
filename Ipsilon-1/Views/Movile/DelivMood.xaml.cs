namespace Ipsilon_1.Views.Movile;
using Ipsilon_1.Views.Movile;
using Ipsilon_1.Models;
using Ipsilon_1.fleshy;
using Ipsilon_1.Views;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Microsoft.Maui.Controls;
using System.Diagnostics;

public partial class DelivMood : ContentPage
{

    double translationX = 0;

    public DelivMood(string ido)
    {
        InitializeComponent();
        Codicia.Text = ido;
        Avaricia.Text = Vars_Globales.Nii;
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();

        if (Vars_Globales.pask != null && Vars_Globales.pask.Estado == 0)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Vars_Globales.pask.Estado = 2;
                string url = $"{Vars_Globales.Uerel}/Paquetes/{Vars_Globales.pask.Id}";
                using (HttpClient client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(Vars_Globales.pask);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    await client.PutAsync(url, content);
                }
                Vars_Globales.pask = null;
            }
            else
            {
                await DisplayAlert("Sin conexión", "No se pudo marcar como no entregado por falta de internet.", "OK");
            }
        }
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await getshit();

        if (Vars_Globales.pask != null)
        {
            Vars_Globales.pask.HorSal = DateTime.Now;
            Mentiras.Text = Vars_Globales.pask.HorSal.ToString("HH:mm");
        }
        else
        {
            await DisplayAlert("Error", "No se encontro el paquete", "OK");
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
                await DisplayAlert("Error", "Error insesperado, intente despues", "OK");
                await Navigation.PopAsync();
            }
        } else
        {
            await DisplayAlert("Error", "No se encontro el servidor", "OK");
            await Navigation.PopAsync();
        }
    }

    public async Task BullshitGo(int stat)
    {
        try
        {
            if (Vars_Globales.pask == null)
            {
                await DisplayAlert("Error", "No se encontro el paquete", "OK");
                return;
            }

            // Detecta conexión a Internet
            var access = Connectivity.NetworkAccess;
            if (access != NetworkAccess.Internet)
            {
                await DisplayAlert("Sin conexión", "No hay conexión a internet intente de nuevo más tarde", "OK");
                return;
            }

            var paquete = new Paquete
            {
                Id = Vars_Globales.pask.Id,
                Repártidor = Vars_Globales.UeserID,
                Codigo = Vars_Globales.pask.Codigo,
                HorSal = Vars_Globales.pask.HorSal,
                HorEnt = stat == 1 ? DateTime.Now : null,
                Estado = stat,
                link = Vars_Globales.pask.link
            };

            string url = $"{Vars_Globales.Uerel}/Paquetes/{paquete.Id}";

            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(paquete);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url, content);

                Vars_Globales.pask = null;

                bool pisc = await DisplayAlert("Entregado", $"Se ha terminado la entrega", "Escanear nuevo paquete", "Cerrar sesión");
                if (pisc)
                {
                    await Navigation.PushAsync(new MovileQr());
                }
                else
                {
                    Vars_Globales.UeserID = 0;
                    Vars_Globales.Nii = null;
                    await Navigation.PopToRootAsync();
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al actualizar el paquete: {ex.Message}", "OK");
        }
    }


    private async void OnSliderPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Running:
                translationX += e.TotalX;
                translationX = Math.Max(-120, Math.Min(120, translationX));
                SliderThumb.TranslationX = translationX;
                break;

            case GestureStatus.Completed:

                if (translationX > 100)
                {
                    bool delivv = await DisplayAlert("Estas Cerrando una entrega", "¿Estas seguro que estas entregando el paquete? si te equivocaste esta accion es irreversible", "Entregar", "Abortar");
                    if (delivv)
                    {
                        await BullshitGo(1);
                    }
                    else
                    {
                        await SliderThumb.TranslateTo(0, 0, 150, Easing.CubicOut);
                        translationX = 0;
                    }
                }
                else if (translationX < -100)
                {

                    bool conf = await DisplayAlert("Estas Cerrando una entrega", "A punto de marcar un paquete como no entregado, esta accion si es reversible", "Si, eso hago", "Cancelar");
                    if (conf)
                    {
                        bool Nivv = await DisplayAlert("Estas Cerrando una entrega", "¿Nadie recibio el paquete o has decidido cancelar la entrega?", "Nadie recibio", "Voy a cancelar la entrega");
                        if (Nivv)
                        {
                            await BullshitGo(2);
                        }
                        else
                        {
                            await BullshitGo(3);
                        }
                    }
                    else
                    {
                        await SliderThumb.TranslateTo(0, 0, 150, Easing.CubicOut);
                        translationX = 0;
                    }
                }

                //reseteo
                await SliderThumb.TranslateTo(0, 0, 150, Easing.CubicOut);
                translationX = 0;
                break;
        }
    }


}