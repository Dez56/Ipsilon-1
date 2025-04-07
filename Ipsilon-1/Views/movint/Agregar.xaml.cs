using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;

namespace Ipsilon_1.Views.movint;

public partial class Agregar : ContentPage
{
	public Agregar()
	{
		InitializeComponent();
	}

    //Revelar forms

    private async void nigga3click(object sender, EventArgs e)
    {
        firstad.IsVisible = true;
        Secadow.IsVisible = false;

    }

    private async void nigga4click(object sender, EventArgs e)
    {
        firstad.IsVisible = false;
        Secadow.IsVisible = true;
    }

    //agregar

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

    private async void OnAgregarPaquetesClicked(object sender, EventArgs e)
    {
        var paquetes = new Paquetez
        {
            Repártidor = Convert.ToInt32(repar.Text),
            Codigo = codig.Text,
            Estado = Convert.ToInt32(stat.Text)
        };

        var resultado = await AgregarpaqueteAsync(paquetes);
        Resultado.Text = resultado ? "Paquete agregado exitosamente" : "Error al agregar usuario";
    }


    //conversor a json y envio a la api

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

    private async Task<bool> AgregarpaqueteAsync(Paquetez Paquete)
    {
        string url = "https://localhost:7169/Paquetes";
        using (HttpClient client = new HttpClient())
        {
            var json = JsonConvert.SerializeObject(Paquete);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            return response.IsSuccessStatusCode;
        }
    }
}

public class Usuario
{
    public required string Nombre { get; set; }
    public required string Contrasena { get; set; }
}
public class Paquetez
{
    public int Id { get; set; }
    public int Repártidor { get; set; }  
    public required string Codigo { get; set; }
    public int Estado { get; set; } 
}
