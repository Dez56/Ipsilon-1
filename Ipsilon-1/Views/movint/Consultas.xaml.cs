using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;

namespace Ipsilon_1.Views.movint;

public partial class Consultas : ContentPage
{
	public Consultas()
	{
		InitializeComponent();
        
    }

    private async void nigga1click (object sender, EventArgs e)
    {
        uno.IsVisible = true;
        tua.IsVisible = false;
        await Load();
    }

    private async void nigga2click (object sender, EventArgs e)
    {
        tua.IsVisible = true;
        uno.IsVisible = false;
        await leed();
    }

    //Los siguientes metodos son consultas a las bases de datos, que llenan el modelo, esto se aprecia en la 4ta linea util de ambos metodos

    private async Task Load()
    {
        string url = "https://localhost:7169/Usuarios"; 
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetStringAsync(url);
            
            var data = JsonConvert.DeserializeObject<List<Usario>>(response);
            DataCollectionView.ItemsSource = data;

        }
    }

    private async Task leed()
    {
        string url = "https://localhost:7169/Paquetes";
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetStringAsync(url);

            var data = JsonConvert.DeserializeObject<List<Paquete>>(response);
            sasas.ItemsSource = data;

        }
    }
}

//modelos usados para llenar los grids

public class Usario
{
    public required int Id { get; set; }
    public required string Nombre { get; set; }
    public required string Contrasena { get; set; }
}

public class Paquete
{
    public int Id { get; set; }
    public int Repártidor { get; set; }  //esta linea en todas sus formas en algun momento se va a joder toda la aplicacion, anota eso Jimmy
    public required string Codigo { get; set; }
    public int Estado { get; set; } // "0 = En proceso de entrega", "1 = Entregado", "Cancelado"
}
