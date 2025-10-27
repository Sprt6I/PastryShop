using PastryServer.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace PastryAdmin;

public partial class Products_Manager_Page : ContentPage
{
	private readonly HttpClient client;

    public ObservableCollection<Product> products_collection { get; set; } = new();

    public Product current_product;

    public Products_Manager_Page()
	{
		InitializeComponent();
        client = new HttpClient { BaseAddress = new Uri("https://localhost:5201/") };

        products_list_view.ItemsSource = products_collection;

        BindingContext = this;

        Load_Products_Async_();
    }

    private async Task Load_Products_Async_()
    {
        try
        {
            var products_list = await client.GetFromJsonAsync<List<Product>>("Auth/GetAllProducts");
            if (products_list != null)
            {
                products_collection.Clear();
                foreach (var product in products_list)
                {
                    products_collection.Add(product);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load products: {ex.Message}", "OK");
        }
    }

    private void Products_List_View_Item_Selectedd(object sender , SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Product selected_product)
        {
            current_product = selected_product;
        }
    }
}