using PastryServer.Models;
using System.Net.Http.Json;

namespace PastryShop
{
    public partial class MainPage : ContentPage
    {
        private static readonly HttpClient client = new() { BaseAddress = new Uri("https://localhost:5201/") };
        public int user_id { get; set; }

        public MainPage(int user_id)
        {
            InitializeComponent();
            this.user_id = user_id;
            Load_Categories_();
        }

        private async void Load_Categories_() // TODO: Group here and remove it from the server
        {
            try
            {
                List<Product_Group>? products_groupedby_category = await client.GetFromJsonAsync<List<Product_Group>>("Products/GetAllProductsGroupedbyCategory");
                if (products_groupedby_category == null)
                {
                    await DisplayAlert("Error", "Failed to load Products.", "OK");
                    return;
                }

                foreach (var group in products_groupedby_category)
                {
                    var category_label = new Label {Text = group.Category};
                    products_layout.Children.Add(category_label);
                    foreach (var product in group.Products)
                    {
                        var product_label = new Label { Text = $"{product.Name} - {product.Description} - In Stock: {product.In_Stock} - Price: {product.Price}$" };
                        products_layout.Children.Add(product_label);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load Products. {ex}", "OK");
                Load_Categories_();
            }
        }

        
    }

}
