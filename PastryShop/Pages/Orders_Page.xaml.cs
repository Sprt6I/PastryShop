namespace PastryShop.Pages;

public partial class Orders_Page : ContentPage
{
    public int user_id;
    public Orders_Page(int user_id)
	{
		InitializeComponent();
		this.user_id = user_id;
	}
}