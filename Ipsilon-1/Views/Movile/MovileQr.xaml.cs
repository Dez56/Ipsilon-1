using Ipsilon_1.Views.Movile;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ZXing;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
using Ipsilon_1.fleshy;
using Ipsilon_1.Models;
using System.Text.Json;
using System.Text;

namespace Ipsilon_1;

public partial class MovileQr : ContentPage
{
    private bool scanningPaused = false;
    public MovileQr()
    {
        InitializeComponent();

        BarcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
        {
            AutoRotate = true,
            Formats = ZXing.Net.Maui.BarcodeFormat.QrCode,
            Multiple = false,
        };
    }

    private void OnBarcodeDetected(object sender, BarcodeDetectionEventArgs e)
    {
        if (scanningPaused) return;

        var First = e.Results?.FirstOrDefault();
        if (First == null) return;

        var value = First.Value;

        if (Uri.TryCreate(value, UriKind.Absolute, out var uri)
            && uri.Host.Contains("facturaelectronica.sat.gob.mx"))
        {
            var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);
            var folioFiscal = queryParams["id"];

            if (!string.IsNullOrEmpty(folioFiscal))
            {
                scanningPaused = true;

                Dispatcher.Dispatch(async () =>
                {
                    try
                    {
                        var httpClient = new HttpClient();
                        var apiUrl = $"{Vars_Globales.Uerel}/Paquetes/BuscarPorCodigo/{folioFiscal}";

                        var response = await httpClient.GetAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonResponse = await response.Content.ReadAsStringAsync();

                            var options = new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            };

                            var paqueteExistente = JsonSerializer.Deserialize<Paquete>(jsonResponse, options);

                            if (paqueteExistente != null)
                            {
                                if (paqueteExistente.Estado == 0)
                                {
                                    await DisplayAlert("Aviso", $"Este paquete ya está en proceso, código: {folioFiscal}", "OK");
                                    return;
                                }
                                else if (paqueteExistente.Estado == 1)
                                {
                                    await Navigation.PushAsync(new delivered(paqueteExistente.Id));
                                    return;
                                }
                            }
                        }
                        else
                        {
                            // Crear paquete si no existe
                            var nuevoPaquete = new Paquete
                            {
                                Repártidor = Vars_Globales.UeserID,
                                Codigo = folioFiscal,
                                Estado = 0,
                                HorSal = DateTime.Now,
                                link = value
                            };

                            var jsonToSend = JsonSerializer.Serialize(nuevoPaquete);
                            var content = new StringContent(jsonToSend, Encoding.UTF8, "application/json");

                            await httpClient.PostAsync($"{Vars_Globales.Uerel}/Paquetes", content);
                        }

                        // Ir a pantalla de entrega
                        await Navigation.PushAsync(new DelivMood(folioFiscal));
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", ex.Message, "OK");
                    }
                    finally
                    {
                        scanningPaused = false;
                    }
                });
            }
        }
    }
}