using System.Net.Http.Json;

namespace PastryAdmin.Pages;

public partial class Admin_Login_Page : ContentPage
{
    private static readonly HttpClient client;

    static Admin_Login_Page()
    {
        HttpClientHandler handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        client = new HttpClient(handler) { BaseAddress = new Uri("https://192.168.1.50:5001/") };
    }

    public Admin_Login_Page()
	{
		InitializeComponent();
	}

	public async void Login_(object sender, EventArgs e)
	{
		string login = login__entry.Text?.Trim() ?? "";
		if (string.IsNullOrWhiteSpace(login)) { error__label.Text = "Please enter your login.";  return; }

		string password = password__entry.Text?.Trim() ?? "";
		if (string.IsNullOrWhiteSpace(password)) { error__label.Text = "Please enter your password."; return; }

        try
		{
            var response = await client.PostAsJsonAsync("Admin/Login", new { login = login, password = password });
            if (!response.IsSuccessStatusCode) { error__label.Text = "Login failed."; return; }
        }
        catch(Exception ex)
		{
            await DisplayAlert("Error", $"Failed to login, server error: {ex.Message}", "OK");
        }


        if (Application.Current == null) { await DisplayAlert("Error", "Failed to update product.", "OK"); return; }
        Application.Current.MainPage = new MainPage();
    }
}