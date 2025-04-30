using Ipsilon_1.Views.Movile;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ZXing;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

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

        // Validar si es un QR del SAT
        if (Uri.TryCreate(value, UriKind.Absolute, out var uri)
            && uri.Host.Contains("facturaelectronica.sat.gob.mx"))
        {
            var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);

            // Verificar si tiene todos los parámetros necesarios
            var folioFiscal = queryParams["id"];
            var rfcEmisor = queryParams["re"];
            var rfcReceptor = queryParams["rr"];
            var total = queryParams["tt"];
            var sello = queryParams["fe"];

            if (!string.IsNullOrEmpty(folioFiscal) &&
                !string.IsNullOrEmpty(rfcEmisor) &&
                !string.IsNullOrEmpty(rfcReceptor) &&
                !string.IsNullOrEmpty(total) &&
                !string.IsNullOrEmpty(sello))
            {
                scanningPaused = true;

                Dispatcher.Dispatch(async () =>
                {
                    await DisplayAlert("QR válido", value, "OK");

                    // Ir a DelivMood pasando los datos
                    await Navigation.PushAsync(new DelivMood(folioFiscal, rfcEmisor, rfcReceptor, total, sello));

                    scanningPaused = false;
                });
            }
        }
    }
}