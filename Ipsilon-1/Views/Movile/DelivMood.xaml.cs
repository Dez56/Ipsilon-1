namespace Ipsilon_1.Views.Movile;

public partial class DelivMood : ContentPage
{
	public DelivMood(string id, string re, string rr, string tt, string fe)
	{
        InitializeComponent();
        T1.Text = id;
        T2.Text = re;
        T3.Text = rr;
        T4.Text = tt;
        T5.Text = fe;
        T6.Text = DateTime.Now.ToString("HH:mm:ss");
    }

    public void ONdeliverance(object sender, EventArgs e)
    {
        DisplayAlert("Entregado", "La entrega ha sido registrada", "OK");
        T7.Text = DateTime.Now.ToString("HH:mm:ss");
        BITTON.IsEnabled = false;
        mensajeado.Text = "El paqute ha sido entregado usted pue. NO NO es cierto, esta pagina no tiene una funcion aparte y no se cierra sola aun, eso esta reservado para la pagina que si guarda registros, solo cierrela manualmente";
    }

}