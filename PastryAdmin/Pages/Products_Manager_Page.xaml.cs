using CommunityToolkit.Maui.Views;
using PastryAdmin.Popups;
using PastryServer.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace PastryAdmin;

public partial class Products_Manager_Page : ContentPage
{
    private static readonly HttpClient client;

    static Products_Manager_Page()
    {
        HttpClientHandler handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        client = new HttpClient(handler) { BaseAddress = new Uri("https://192.168.0.31:5001/") };
    }

    public ObservableCollection<Product> products_collection { get; set; } = new();

    public Product current_product = new Product();

    public Products_Manager_Page()
	{
		InitializeComponent();

        products__list_view.ItemsSource = products_collection;

        //BindingContext = this;
        products__list_view.ItemsSource = products_collection;

        Load_Product_Categories_();
        Load_Products_();
    }

    public async void Add_Product_(object sender, EventArgs e)
    {
        string product_name = product_name__entry.Text?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(product_name)) { await DisplayAlert("Error", "Add product name", "OK"); return; }

        string product_description = product_description__entry.Text?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(product_description)) { await DisplayAlert("Error", "Add product description", "OK"); return; }

        string product_category = product_category__picker.SelectedItem?.ToString() ?? "";
        if (string.IsNullOrWhiteSpace(product_category)) { await DisplayAlert("Error", "Add product category", "OK"); return; }

        string how_many_in_stock = how_many_in_stock__entry.Text?.Trim() ?? "";
        if (string.IsNullOrEmpty(how_many_in_stock)) { await DisplayAlert("Error", "Add how many in stock", "OK"); return; }
        int how_many_in_stock__int = int.Parse(how_many_in_stock);

        string product_price = product_price__entry.Text?.Trim() ?? "";
        if (string.IsNullOrEmpty(product_price)) { await DisplayAlert("Error", "Add product price", "OK"); return; }
        float product_price__int = float.Parse(product_price);

        try
        {
            await client.PostAsJsonAsync("Products/AddProduct", new { Name=product_name, Description=product_description, Category=product_category, In_Stock=how_many_in_stock__int, Price= product_price__int});
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Server error {ex}", "Ok");
            return;
        }


        await Load_Products_();
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

    public async void Update_Product_(Product product)
    {
        HttpResponseMessage? response = null;
        try
        {
            response = await client.PostAsJsonAsync("Products/UpdateProducts", product);
            if (response == null)
            {
                await DisplayAlert("Error", "Server error", "Ok");
                return;
            }
        }
        catch (Exception ex) {
            await DisplayAlert("Error", $"Server error {ex}", "Ok");
            return;
        }

        if (!response.IsSuccessStatusCode)
        {
            await DisplayAlert("Error", "Failed to update product.", "OK"); return;
        }

        Load_Products_();
    } 

    public async void Product_Selection_Changed_(object sender, SelectionChangedEventArgs e)
    {
        Product selected_product = (Product)e.CurrentSelection.FirstOrDefault() ?? null;
        if (selected_product == null) { return; }

        current_product = selected_product;

        var popup = new Product_Popup(selected_product);
        await this.ShowPopupAsync(popup);

        Update_Product_(current_product);

        ((CollectionView)sender).SelectedItem = null;
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