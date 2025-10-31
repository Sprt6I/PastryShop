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

    public void How_Many_In_Stock_Entry_Text_Changed_(object sender, TextChangedEventArgs e)
    {
        var entry = (Entry)sender;
        string new_text = e.NewTextValue;
        if (string.IsNullOrEmpty(new_text)) { return; }
        if (!System.Text.RegularExpressions.Regex.IsMatch(new_text, @"^[0-9]*$"))
        {
            entry.Text = e.OldTextValue;
        }
    }

    public void Price_Entry_Text_Changed_(object sender, TextChangedEventArgs e)
    {
        var entry = (Entry)sender;
        string new_text = e.NewTextValue;
        if (string.IsNullOrEmpty(new_text)) { return; }
        if (!System.Text.RegularExpressions.Regex.IsMatch(new_text, @"^([0-9]+\.?[0-9]{0,2}|[0-9]*\.[0-9]{1,2})$"))
        {
            entry.Text = e.OldTextValue;
        }
    }
}