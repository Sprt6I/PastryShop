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

        products__list_view.ItemsSource = products_collection;

        BindingContext = this;

        Load_Product_Categories_();
        Load_Products_();
    }

    public async void Add_Product_(object sender, EventArgs e)
    {
        string product_name = product_name__entry.Text?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(product_name)) { return; }

        string product_description = product_description__entry.Text?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(product_description)) { return; }

        string product_category = product_category__picker.SelectedItem?.ToString() ?? "";
        if (string.IsNullOrWhiteSpace(product_category)) { return; }

        string how_many_in_stock = how_many_in_stock__entry.Text?.Trim() ?? "";
        if (string.IsNullOrEmpty(how_many_in_stock)) { return; }
        int how_many_in_stock__int = int.Parse(how_many_in_stock);

        await client.PostAsJsonAsync("Products/AddProduct", new { Name=product_name, Description=product_description, Category=product_category, In_Stock=how_many_in_stock__int});
    }

    private async Task Load_Product_Categories_()
    {
        try
        {
            var categories_list = await client.GetFromJsonAsync<List<Product_Category>>("Products/GetAllProductCategories");
            if (categories_list != null)
            {
                product_category__picker.Items.Clear();
                foreach (var category in categories_list)
                {
                    product_category__picker.Items.Add(category.Name);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load product categories: {ex.Message}", "OK");
        }
    }

    private async Task Load_Products_()
    {
        try
        {
            var products_list = await client.GetFromJsonAsync<List<Product>>("Products/GetAllProducts");
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

    public async void Update_Products_(object sender, EventArgs e)
    {
        if (current_product == null)
        {
            await DisplayAlert("Error", "No product selected.", "OK");
            return;
        }

        var response = await client.PostAsJsonAsync("Products/UpdateProducts", products_collection.ToList());

        if (!response.IsSuccessStatusCode)
        {
            await DisplayAlert("Error", "Failed to update product.", "OK"); return;
        }

    } 

    public void Product_Selection_Changed_(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Product selected_product)
        {
            current_product = selected_product;
        }
    }
}