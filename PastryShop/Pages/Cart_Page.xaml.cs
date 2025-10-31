using System.Net.Http.Json;

namespace PastryShop.Pages;
using PastryServer.Models;

public partial class Cart_Page : ContentPage
{
    private static readonly HttpClient client = new HttpClient { BaseAddress = new Uri("https://localhost:5201/") };
    public int user_id { get; set; }
    public Cart_Page(int user_id)
	{
		InitializeComponent();
		this.user_id = user_id;
	}
	public async Task Load_User_Cart()
	{
        HttpResponseMessage response = new();
        try
        {
            response = await client.PostAsJsonAsync($"Cart/GetCart", new { user_id = user_id });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Server error, failed to load cart. {ex}", "OK");
            return;
        }

        if (!response.IsSuccessStatusCode)
        {
            await DisplayAlert("Error", "Failed to load cart.", "OK");
            return;
        }

        User_Cart? user_cart = await response.Content.ReadFromJsonAsync<User_Cart>();
        if (user_cart == null)
        {
            await DisplayAlert("Error", "Failed to parse cart data.", "OK");
            return;
        }

        foreach (Bought_Product product in user_cart.Bought_Products) {
            var product_label = new Label { Text = $"{product.Product_Id} - {product.Quantity}" };
            user_cart_layout.Children.Add(product_label);
        }
    }
}