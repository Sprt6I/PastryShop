namespace PastryShop.Pages;

public partial class Cart_Page : ContentPage
{
    public int user_id;
    public Cart_Page(int user_id)
	{
		InitializeComponent();
		this.user_id = user_id;
	}
	public async Task Load_User_Cart()
	{

	}
}