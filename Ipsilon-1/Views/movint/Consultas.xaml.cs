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

    private void HideALLGs(string SelecLay)
    {
        var layouts = new[] { "uno", "tua", "AgregarGrupoUser" };

        foreach (var layoutName in layouts)
        {
            var layout = this.FindByName<Layout>(layoutName);
            if (layout != null)
            {
                layout.IsVisible = layoutName == SelecLay;
            }
        }
    }

    private async void nigga1click (object sender, EventArgs e)
    {
        HideALLGs("uno");
        uno.IsVisible = true;
        await Load();
    }

    private async void nigga2click (object sender, EventArgs e)
    {
        HideALLGs("tua");
        tua.IsVisible = true;
        await leed();
    }

    private  void RevealUserAdd(object sender, EventArgs e)
    {
        HideALLGs("AgregarGrupoUser");
    }

    private void BuscaUsers(object sender, EventArgs e)
    {
        if (true)
        {
            return;
        }
    }

    private void RevealPaqueAdd(object sender, EventArgs e)
    {
        if (true)
        {
            return;
        }
    }

    private void BuscaPaque(object sender, EventArgs e)
    {
        if (true)
        {
            return;
        }
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
        string urlPaquetes = "https://localhost:7169/Paquetes";
        string urlRepartidores = "https://localhost:7169/Usuarios";

        using (HttpClient client = new HttpClient())
        {
            var responsePaquetes = await client.GetStringAsync(urlPaquetes);
            var paquetes = JsonConvert.DeserializeObject<List<Paquete>>(responsePaquetes);

            var responseRepartidores = await client.GetStringAsync(urlRepartidores);
            var repartidores = JsonConvert.DeserializeObject<List<Usuario>>(responseRepartidores);

            foreach (var paquete in paquetes)
            {
                var repartidor = repartidores.FirstOrDefault(r => r.Id == paquete.Repártidor);
                paquete.NombreRepartidor = repartidor?.Nombre ?? "Desconocido";
            }

            sasas.ItemsSource = paquetes;
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
        await DisplayAlert("Se ha agregado un usuario", "Usuario agregado exitosamente", "OK");
        NombreEntry.Text = string.Empty;
        ContrasenaEntry.Text = string.Empty;
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

    //Metodos de edicion de tablas

    //Usuarios edit

    private async void OnEditarUsuarioClicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        var usuario = button?.CommandParameter as Usuario;

        if (usuario != null)
        {
            // Mostrar un formulario para editar al usuario
            await DisplayAlert("Editar Usuario", $"Editarás al usuario: {usuario.Nombre}", "OK");

            // Aquí puedes implementar la lógica para mostrar un formulario de edición
            // y actualizar los datos del usuario.
        }
    }

    //Paquetes edit

    private async void OnEditarPaqueClicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        var usuario = button?.CommandParameter as Usuario;

        if (usuario != null)
        {
            // Mostrar un formulario para editar al usuario
            await DisplayAlert("Editar Usuario", $"Editarás al usuario: {usuario.Nombre}", "OK");

            // Aquí puedes implementar la lógica para mostrar un formulario de edición
            // y actualizar los datos del usuario.
        }
    }

    //Metodos de Eliminacion de registros

    //Elimniar usuarios
    private async void OnEliminarUsuarioClicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        var usuario = button?.CommandParameter as Usuario;

        if (usuario != null)
        {
            var confirm = await DisplayAlert("Confirmar Eliminación", $"Seguro de que quieres eliminar a {usuario.Nombre}?", "Sí", "No");

            if (confirm)
            {
                // Llamar a la API para eliminar al usuario
                string url = $"https://localhost:7169/Usuarios/{usuario.Id}";
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.DeleteAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Usuario Eliminado", $"El usuario {usuario.Nombre} ha sido eliminado.", "OK");

                        // Recargar la tabla de usuarios
                        await Load();
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo eliminar al usuario.", "OK");
                    }
                }
            }
        }
    }

    //Eliminar paquetes
    private async void OnEliminarPaqueClicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        var paque = button?.CommandParameter as Paquete;

        if (paque != null)
        {
            var confirm = await DisplayAlert("Confirmar Eliminación", $"Seguro de que quieres eliminar a {paque.Codigo}?", "Sí", "No");

            if (confirm)
            {
                // Llamar a la API para eliminar al usuario
                string url = $"https://localhost:7169/Paquetes/{paque.Id}";
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.DeleteAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Usuario Eliminado", $"El usuario {paque.Codigo} ha sido eliminado.", "OK");

                        // Recargar la tabla de usuarios
                        await Load();
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo eliminar al usuario.", "OK");
                    }
                }
            }
        }
    }
}

/*Modelos para el envio de formularios, esto es algo un poco muy
 polimorfo ya que se usa el mismo para el recibir y enviar Jsons
para el API, el plan es que esto este en el mismo archivo*/

public class Usuario
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public required string Contrasena { get; set; }
}

public class Paquete
{
    public int Id { get; set; }
    public int Repártidor { get; set; }  //esta linea en todas sus formas en algun momento se va a joder toda la aplicacion, anota eso Jimmy
    public required string? NombreRepartidor { get; set; }
    public required string Codigo { get; set; }
    public int Estado { get; set; } 

    public string EstadoDescripcion
    {
        get
        {
            return Estado switch
            {
                0 => "En proceso de entrega",
                1 => "Entregado",
                2 => "Cancelado",
                3 => "No entregado",
                _ => throw new NotImplementedException()
            };
        }
    }
}
