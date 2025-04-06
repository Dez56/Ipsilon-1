using System.Security.Cryptography;

namespace Ipsilon_1;

public partial class MovileQr : ContentPage
{
	public MovileQr(string nombre)
	{	
		InitializeComponent();

		sesa.Text = nombre;
	}
}