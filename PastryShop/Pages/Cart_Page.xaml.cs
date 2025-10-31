using System.Net.Http.Json;

namespace PastryShop.Pages;
using PastryServer.Models;
using System.Collections.Generic;

public partial class Cart_Page : ContentPage
{
    private static readonly HttpClient client;

    static Cart_Page()
    {
        HttpClientHandler handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        client = new HttpClient(handler) { BaseAddress = new Uri("https://192.168.0.31:5001/") };
    }
    public int user_id { get; set; }
    public Cart_Page(int user_id)
	{
		InitializeComponent();
        this.user_id = user_id;
        _ = Load_User_Cart();

    }
	public async Task Load_User_Cart()
	{
        HttpResponseMessage? response = null;
        User_Cart? user_cart = null;
        try
        {
            response = await client.PostAsJsonAsync($"Auth/GetCart", new { user_id = user_id });
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

        if (response == null)
        {
            await DisplayAlert("Error", "Failed to load cart response.", "OK");
            return;
        }

        user_cart = await response.Content.ReadFromJsonAsync<User_Cart>();
        if (user_cart == null)
        {
            await DisplayAlert("Error", "Failed to parse cart data.", "OK");
            return;
        }

        List <Product>? products_list = null;
        try
        {
            products_list = await client.GetFromJsonAsync<List<Product>>("Products/GetAllProducts");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load products: {ex.Message}", "OK");
        }
        if (products_list == null) { await DisplayAlert("Error", "Failed to load products.", "OK"); return; }

        foreach (Bought_Product bought_product in user_cart.Bought_Products) {
            Product? product = products_list.Find(p => p.Id == bought_product.Product_Id);
            if (product == null) { continue; }
            var product_label = new Label { Text = $"{product.Name} - {product.Price}" };
            user_cart_layout.Children.Add(product_label);
        }
    }
}