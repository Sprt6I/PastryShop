namespace PastryShop.Pages;
using PastryServer.Helper_Files;

public partial class Orders_Page : ContentPage
{
    private static readonly HttpClient client;

    static Orders_Page()
    {
        string ip = Checks.Get_Ipv4_();
        HttpClientHandler handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        client = new HttpClient(handler) { BaseAddress = new Uri($"https://{ip}:5001/") };
    }
    public int user_id { get; set; }
    public Orders_Page(int user_id)
	{
		InitializeComponent();
		this.user_id = user_id;
	}
}