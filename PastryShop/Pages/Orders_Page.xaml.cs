namespace PastryShop.Pages;

public partial class Orders_Page : ContentPage
{
    private static readonly HttpClient client;

    static Orders_Page()
    {
        HttpClientHandler handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        client = new HttpClient(handler) { BaseAddress = new Uri("https://192.168.0.31:5001/") };
    }
    public int user_id { get; set; }
    public Orders_Page(int user_id)
	{
		InitializeComponent();
		this.user_id = user_id;
	}
}