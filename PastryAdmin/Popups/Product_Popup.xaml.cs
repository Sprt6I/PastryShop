using CommunityToolkit.Maui.Views;
using PastryServer.Models;
namespace PastryAdmin.Popups;

public partial class Product_Popup : Popup
{
	public Product_Popup(Product binded_product)
	{
		InitializeComponent();
		BindingContext = binded_product;
	}

	private void Save_Clicked_(object sender, EventArgs e)
	{
		Close(BindingContext);
	}
}