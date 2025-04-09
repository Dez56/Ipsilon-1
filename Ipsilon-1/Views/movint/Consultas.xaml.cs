using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.Text;

namespace Ipsilon_1.Views.movint;

public partial class Consultas : ContentPage
{
	public Consultas()
	{
		InitializeComponent();
        
    }

    /* Botones reveladores de forms */

    private void HideALLGs()
    {
        uno.IsVisible = false;
        tua.IsVisible = false;
        AgregarGrupoUser.IsVisible = false;
    }

    private async void nigga1click (object sender, EventArgs e)
    {
        HideALLGs();
        uno.IsVisible = true;
        await Load();
    }

    private async void nigga2click (object sender, EventArgs e)
    {
        HideALLGs();
        tua.IsVisible = true;
        await leed();
    }

    private void RevealUserAdd(object sender, EventArgs e)
    {
        HideALLGs();
        AgregarGrupoUser.IsVisible = true;

    }

    /*Los siguientes metodos son consultas a las bases de datos, que llenan el modelo
     esto se aprecia en la 4ta linea util de ambos metodos*/

    private async Task Load()
    {
        string url = "https://localhost:7169/Usuarios"; 
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetStringAsync(url);
            
            var data = JsonConvert.DeserializeObject<List<Usuario>>(response);
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

    /*Metodos de edicion de tablas*/

    //Metodos para agregar registross

        //Usuarios Main metod

    private async void OnAgregarUsuarioClicked(object sender, EventArgs e)
    {
        var usuario = new Usuario
        {
            Nombre = NombreEntry.Text,
            Contrasena = ContrasenaEntry.Text
        };

        var resultado = await AgregarUsuarioAsync(usuario);
        ResultadoLabel.Text = resultado ? "Usuario agregado exitosamente" : "Error al agregar usuario";
    }

        //Usuario util task
    private async Task<bool> AgregarUsuarioAsync(Usuario usuario)
    {
        string url = "https://localhost:7169/Usuarios";
        using (HttpClient client = new HttpClient())
        {
            var json = JsonConvert.SerializeObject(usuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            return response.IsSuccessStatusCode;
        }
    }
}

/*Modelos para el envio de formularios, esto es algo un poco muy
 polimorfo ya que se usa el mismo para el recibir y enviar Jsons
para el API, el plan es que esto este en el mismo archivo*/

public class Usuario
{
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
