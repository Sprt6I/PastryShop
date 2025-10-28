using BCrypt.Net;
using System.Net.Http.Json;

namespace PastryAdmin.Pages;

public partial class Admin_Login_Page : ContentPage
{
    private static readonly HttpClient client = new HttpClient { BaseAddress = new Uri("https://localhost:5201/") };
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

        var response = await client.PostAsJsonAsync("Admin/Login", new {login=login, password=password});
		if (!response.IsSuccessStatusCode) { error__label.Text = "Login failed."; return; }

        await Navigation.PushAsync(new MainPage());
    }
}