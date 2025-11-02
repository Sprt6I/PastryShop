using PastryServer.Requests;
using System.Net.Http.Json;

namespace PastryShop
{
    public partial class MainPage : ContentPage
    {
        private static readonly HttpClient client;

        static MainPage()
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            client = new HttpClient(handler) { BaseAddress = new Uri("https://192.168.1.50:5001/") };
        }
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
                    var category_label = new Label {
                        Text = group.Category,
                        Style = (Style)Application.Current.Resources["CategoryLable"]
                    };
                    products_layout.Children.Add(category_label);
                    foreach (var product in group.Products)
                    {
                        if (product.In_Stock > 0) // Remove == Break app, pls don't remove ( cuz in Product_Widget if there is less then it stock it will go into infinite look, changing text )
                        {
                            products_layout.Children.Add(new Custom_Widgets.Product_Widget
                            {
                                product_id = product.Id,
                                product_name = product.Name,
                                product_description = product.Description,
                                product_in_stock = product.In_Stock,
                                product_price = (int)product.Price,
                                client = client,
                            });
                        }
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
