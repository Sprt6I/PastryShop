using System.Net.Http.Json;

namespace PastryShop.Pages;
using PastryServer.Models;
using System.Collections.Generic;

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
        User_Cart? user_cart = null;
        try
        {
            response = await client.PostAsJsonAsync($"Cart/GetCart", new { user_id = user_id });

            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "Failed to load cart.", "OK");
                return;
            }

            user_cart = await response.Content.ReadFromJsonAsync<User_Cart>();
            if (user_cart == null)
            {
                await DisplayAlert("Error", "Failed to parse cart data.", "OK");
                return;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Server error, failed to load cart. {ex}", "OK");
            return;
        }

        List <Product>? products_list = null;
        try
        {
            products_list = await client.GetFromJsonAsync<List<Product>>("Products/GetAllProducts");
            if (products_list == null) { await DisplayAlert("Error", "Failed to load products.", "OK"); return; }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load products: {ex.Message}", "OK");
        }

        foreach (Bought_Product bought_product in user_cart.Bought_Products) {
            Product? product = products_list.Find(p => p.Id == bought_product.Product_Id);
            var product_label = new Label { Text = $"{product.Name} - {product.Price}" };
            user_cart_layout.Children.Add(product_label);
        }
    }
}