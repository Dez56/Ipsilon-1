using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Ipsilon_1.Models;
using Ipsilon_1.fleshy;
using Newtonsoft.Json;
using System.Text;


namespace Ipsilon_1.Views.movint;

public partial class Consultas : ContentPage
{

    private int currentPage = 0;
    private const int pageSize = 50;
    private int currentPagePaquetes = 0;
    private const int pageSizePaquetes = 50;
    public Consultas()
	{
		InitializeComponent();
        currentPage = Preferences.Get("paginaUsuarios", 0);
        currentPagePaquetes = Preferences.Get("paginaPaquetes", 0);

    }

    /* Botones reveladores de forms */

    private void HideALLGs(string SelecLay)
    {
        var layouts = new[] { "uno", "tua", "AgregarGrupoUser", "AgregarGrupoPaq", "EditarGrupoUser", "EditarGrupoPaq" };

        foreach (var layoutName in layouts)
        {
            var layout = this.FindByName<Layout>(layoutName);
            if (layout != null)
            {
                layout.IsVisible = layoutName == SelecLay;
            }
        }

        currentPage = 0;
        
    }

    //botobnes sigs usuarios

    private async void OnNextPageClicked(object sender, EventArgs e)
    {
        currentPage++;
        Preferences.Set("paginaUsuarios", currentPage); 
        await Load();
    }

    private async void OnPreviousPageClicked(object sender, EventArgs e)
    {
        if (currentPage > 0)
        {
            currentPage--;
            Preferences.Set("paginaUsuarios", currentPage); 
            await Load();
        }
    }

    private async void OnSiguientePaqueteClicked(object sender, EventArgs e)
    {
        currentPagePaquetes++;
        Preferences.Set("paginaPaquetes", currentPagePaquetes);
        await leed();
    }

    private async void OnAnteriorPaqueteClicked(object sender, EventArgs e)
    {
        if (currentPagePaquetes > 0)
        {
            currentPagePaquetes--;
            Preferences.Set("paginaPaquetes", currentPagePaquetes); 
            await leed();
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

    //En deshuso
    private void BuscaUsers(object sender, EventArgs e)
    {
        if (true)
        {
            return;
        }
    }

    private void RevealPaqueAdd(object sender, EventArgs e)
    {
        HideALLGs("AgregarGrupoPaq");
        AgregarGrupoPaq.IsVisible = true;
    }

    //En desuso
    private void BuscaPaque(object sender, EventArgs e)
    {
        if (true)
        {
            return;
        }
    }

    //somethin'

    private async void OnAbrirLinkClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton btn && btn.CommandParameter is Paquete paquete)
        {
            bool confirm = await DisplayAlert("Link de Factura", "Estas saliendo en direcci�n a la pagina del sat para ver esta factura a dteale", "As� es", "cancelar");

            if (confirm)
            {
                try
                {
                    await Launcher.Default.OpenAsync(new Uri(paquete.link));
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"No se pudo abrir el enlace: {ex.Message}", "OK");
                }
            }
        }
    }

    //Validacion y llamada a form de edicion de usuarios
    private async void OnSHEUsuarioClicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        var usuario = button?.CommandParameter as Usuario;

        if (usuario != null)
        {
            await DisplayAlert("Editar Usuario", $"Editar�s al usuario: {usuario.Nombre}", "OK");

            HideALLGs("EditarGrupoUser");
            EditarGrupoUser.IsVisible = true;

            HidedIDUser.Text = usuario.Id.ToString();
            EditNombreEntry.Text = usuario.Nombre;
            EditContrasenaEntry.Text = usuario.Contrasena;
        }
    }

    private void OnEditarPaqueClicked(object sender, EventArgs e)
    {
        var bothon = sender as ImageButton;
        var paquete = bothon?.CommandParameter as Paquete;
        if (paquete != null)
        {
            DisplayAlert("Editar Paquete", $"Editar�s al paquete: {paquete.Codigo} llevado por {paquete.NombreRepartidor}", "OK");
            HideALLGs("EditarGrupoPaq");
            EditarGrupoPaq.IsVisible = true;

            var gadio = paquete.Estado;

            switch (gadio)
            {
                case 0:
                    tA0.IsChecked = true;
                    break;
                case 1:
                    tA1.IsChecked = true;
                    break;
                case 2:
                    tA2.IsChecked = true;
                    break;
                case 3:
                    tA3.IsChecked = true;
                    break;
            }

            HideditIDPaq.Text = paquete.Id.ToString();
            HidedsaL.Text = paquete.HorSal.ToString("yyyy-MM-dd HH:mm"); // Formato editable
            HidedEnt.Text = paquete.HorEnt.HasValue ? paquete.HorEnt.Value.ToString("yyyy-MM-dd HH:mm") : string.Empty;

            Paquid.Text = Convert.ToString(paquete.Rep�rtidor);
            Paqueter.Text = paquete.Codigo;
            L2nk.Text = paquete.link;

        }
    }

    /*Los siguientes metodos son consultas a las bases de datos, que llenan el modelo
     esto se aprecia en la 4ta linea util de ambos metodos*/

    private async Task Load()
    {
        int skip = currentPage * pageSize;
        string url = $"{Vars_Globales.Uerel}/Usuarios?skip={skip}&take={pageSize}";

        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<List<Usuario>>(response);
            DataCollectionView.ItemsSource = data;
        }   
    }

    private async Task leed()
    {
        int skip = currentPagePaquetes * pageSizePaquetes;
        string urlPaquetes = $"{Vars_Globales.Uerel}/Paquetes?skip={skip}&take={pageSizePaquetes}";
        string urlRepartidores = $"{Vars_Globales.Uerel}/Usuarios";

        using (HttpClient client = new HttpClient())
        {
            var responsePaquetes = await client.GetStringAsync(urlPaquetes);
            var paquetes = JsonConvert.DeserializeObject<List<Paquete>>(responsePaquetes) ?? new List<Paquete>();

            var responseRepartidores = await client.GetStringAsync(urlRepartidores);
            var repartidores = JsonConvert.DeserializeObject<List<Usuario>>(responseRepartidores) ?? new List<Usuario>();

            foreach (var paquete in paquetes)
            {
                var repartidor = repartidores.FirstOrDefault(r => r.Id == paquete.Rep�rtidor);
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

        if (string.IsNullOrWhiteSpace(NombreEntry.Text) ||
        string.IsNullOrWhiteSpace(ContrasenaEntry.Text))
        {
            await DisplayAlert("Error", "Falta uno o m�s campos", "OK");
            return;
        }
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
        string url = $"{Vars_Globales.Uerel}/Usuarios";
        using (HttpClient client = new HttpClient())
        {
            var json = JsonConvert.SerializeObject(usuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            return response.IsSuccessStatusCode;
        }
    }

    //Paquetes Main metod

    private async void OnAgregarPaquetesClicked(object sender, EventArgs e)
    {
        var RadiValor = 0;


        if (RA0.IsChecked)
        {
            RadiValor = 0;
        }
        else if (RA1.IsChecked)
        {
            RadiValor = 1;
        }
        else if (RA2.IsChecked)
        {
            RadiValor = 2;
        }
        else if (RA3.IsChecked)
        {
            RadiValor = 3;
        }
        else
        {
            await DisplayAlert("Error", "Selecciona un estado", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(repar.Text) ||
        string.IsNullOrWhiteSpace(codig.Text) ||
        string.IsNullOrWhiteSpace(l1nk.Text))
        {
            await DisplayAlert("Error", "Falta uno o m�s campos", "OK");
        }

        var paquetes = new Paquete
        {
            Rep�rtidor = Convert.ToInt32(repar.Text),
            Codigo = codig.Text,
            Estado = RadiValor,
            HorSal = DateTime.Now,
            HorEnt = RadiValor == 1 ? DateTime.Now : null,
            link = l1nk.Text
        };

        var resultado = await AgregarpaqueteAsync(paquetes);
        await DisplayAlert("Alerta", "Registro Agregad�", "ok");
        repar.Text = "";
        codig.Text = "";
        l1nk.Text = "";
        switch (RadiValor)
        {
            case 0:
                RA0.IsChecked = false;
                break;
            case 1:
                RA1.IsChecked = false;
                break;
            case 2:
                RA2.IsChecked = false;
                break;
            case 3:
                RA3.IsChecked = false;
                break;
        }

    }

    //Paquetes util task

    private async Task<bool> AgregarpaqueteAsync(Paquete Paquete)
    {
        string url = $"{Vars_Globales.Uerel}/Paquetes";
        using (HttpClient client = new HttpClient())
        {
            var json = JsonConvert.SerializeObject(Paquete);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            return response.IsSuccessStatusCode;
        }
    }

    //Metodos de edicion de tablas

    //Usuarios edit

    private async void OnEditarUsuarioClicked(object sender, EventArgs e)
    {
        var usuario = new Usuario
        {
            Id = Convert.ToInt32(HidedIDUser.Text),
            Nombre = EditNombreEntry.Text,
            Contrasena = EditContrasenaEntry.Text
        };

        string url = $"{Vars_Globales.Uerel}/Usuarios/{usuario.Id}"; 
        using (HttpClient client = new HttpClient())
        {
            var json = JsonConvert.SerializeObject(usuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(url, content);

            await DisplayAlert("Terminado", "El usuario se ha modificado correctamente.", "OK");
        }

        HideALLGs("uno");
        await Load();
    }

    //Paquetes edit

    private async void Packaginger(object sender, EventArgs e)
    {
        var paquete = new Paquete
        {
            Id = Convert.ToInt32(HideditIDPaq.Text),
            Rep�rtidor = Convert.ToInt32(Paquid.Text),
            Codigo = Paqueter.Text,
            HorSal = DateTime.Parse(HidedsaL.Text),
            HorEnt = string.IsNullOrWhiteSpace(HidedEnt.Text) ? (DateTime?)null : DateTime.Parse(HidedEnt.Text),
            Estado = tA0.IsChecked ? 0 : tA1.IsChecked ? 1 : tA2.IsChecked ? 2 : 3,
            link = L2nk.Text
        };
        string url = $"{Vars_Globales.Uerel}/Paquetes/{paquete.Id}"; // Endpoint de la API
        using (HttpClient client = new HttpClient())
        {
            var json = JsonConvert.SerializeObject(paquete);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(url, content);
            await DisplayAlert("Terminado", $"El paquete {Paqueter.Text}", "OK");
        }
        HideALLGs("tua");
        await leed();
    }

    //Metodos de Eliminacion de registros

    //Elimniar usuarios
    private async void OnEliminarUsuarioClicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        var usuario = button?.CommandParameter as Usuario;

        if (usuario != null)
        {
            var confirm = await DisplayAlert("Confirmar Eliminaci�n", $"Seguro de que quieres eliminar a {usuario.Nombre}?", "S�", "No");

            if (confirm)
            {
                // Llamar a la API para eliminar al usuario
                string url = $"{Vars_Globales.Uerel}/Usuarios/{usuario.Id}";
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
            var confirm = await DisplayAlert("Confirmar Eliminaci�n", $"Seguro de que quieres eliminar a {paque.Codigo}?", "S�", "No");

            if (confirm)
            {
                // Llamar a la API para eliminar al usuario
                string url = $"{Vars_Globales.Uerel}/Paquetes/{paque.Id}";
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.DeleteAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Usuario Eliminado", $"El paquete {paque.Codigo} ha sido eliminado.", "OK");

                        // Recargar la tabla de usuarios
                        await leed();
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