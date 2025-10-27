namespace PastryShop;
using Microsoft.Maui.Controls;
using System.Net.Http.Json;
using PastryServer.Helper_Files;

public partial class Login_Page : ContentPage
{
    private static readonly HttpClient client = new HttpClient { BaseAddress = new Uri("https://localhost:5201/") };
    
    
    public Login_Page()
	{
		InitializeComponent();
	}

    public async void Login_(object sender, EventArgs e)
    {
        string gmail = login_gmail_entry.Text?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(gmail)) { login_errors_label.Text = "Gmail Can't be empty"; return; }
        if (!Checks.Is_Gmail_Valid_(gmail)) { login_errors_label.Text = "Gmail must be correct"; return; }

        string password = login_password_entry.Text?.Trim() ?? "";
        //login_password_entry.Text = "";
        if (string.IsNullOrWhiteSpace(password)) { login_errors_label.Text = "password can't be empty"; return; }
        if (!Checks.Is_Password_Valid_(password)) { login_errors_label.Text = "password must be valid"; return; }
            
        var password_hash = BCrypt.Net.BCrypt.HashPassword(password);
        var response = await client.PostAsJsonAsync("Auth/Login", new { gmail = gmail, password = password_hash });

        if (!response.IsSuccessStatusCode) { login_errors_label.Text = await response.Content.ReadAsStringAsync();  return; }

        login_errors_label.Text = "logged";
        login_gmail_entry.Text = "";
        login_password_entry.Text = "";
        // Go_To_Main_App
    }

    public void Forgot_Password_(object sender, EventArgs e)
    {

    }

    public async void Go_To_Register_(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Register_Page());
    }
}