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

    private async void Load(object sender, EventArgs e)
    {
        string url = "http://localhost:5015/Usuarios"; 
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetStringAsync(url);
            
            var data = JsonConvert.DeserializeObject<List<Usario>>(response);
            DataCollectionView.ItemsSource = data;

        }
    }
}

public class Usario
{
    public required int Id { get; set; }
    public required string Nombre { get; set; }
    public required string Contrasena { get; set; }
}
