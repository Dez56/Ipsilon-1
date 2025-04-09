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

    /*Botones reveladores de forms*/

    private void nigga3click(object sender, EventArgs e)
    {
        AgregarGrupoUser.IsVisible = true;
        AgregarGrupoPaq.IsVisible = false;

    }

    private void nigga4click(object sender, EventArgs e)
    {
        AgregarGrupoUser.IsVisible = false;
        AgregarGrupoPaq.IsVisible = true;
    }

    /*Botones de accion*/

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
        // Encuentra el RadioButton seleccionado usando LINQ
        var selectedRadioButton = AgregarGrupoPaq.Children
            .OfType<RadioButton>()
            .FirstOrDefault(rb => rb.IsChecked);

        // Obtén el valor del RadioButton seleccionado
        var selectedValue = selectedRadioButton?.Value;

        // Crea el 

        var paquetes = new Paquetez
        {
            Repártidor = Convert.ToInt32(repar.Text),
            Codigo = codig.Text,
            Estado = Convert.ToInt32(selectedValue)
        };

        var resultado = await AgregarpaqueteAsync(paquetes);
        Resultado.Text = resultado ? "Paquete agregado exitosamente" : "Error al agregar usuario";
    }


    /*Apartado de utileria, en este caso conversor de datos a archivos
     Json*/

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


public class Paquetez
{
    public int Id { get; set; }
    public int Repártidor { get; set; }  
    public required string Codigo { get; set; }
    public int Estado { get; set; } 
}
