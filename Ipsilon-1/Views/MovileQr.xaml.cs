using System.Security.Cryptography;
using System.Threading.Tasks;
using ZXing;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace Ipsilon_1;

public partial class MovileQr : ContentPage
{
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
        var First = e.Results?.FirstOrDefault();

        if (First == null)
        {
            return;
        }

        Dispatcher.DispatchAsync(async () =>
        {
            await DisplayAlert("QR Code", First.Value, "OK");
        });
    }

    
}