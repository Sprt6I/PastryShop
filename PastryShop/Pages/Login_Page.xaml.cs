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
        if (string.IsNullOrWhiteSpace(password)) { login_errors_label.Text = "password can't be empty"; return; }
        if (!Checks.Is_Password_Valid_(password)) { login_errors_label.Text = "password must be valid"; return; }

        HttpResponseMessage response = null;
        try
        {
            response = await client.PostAsJsonAsync("Auth/Login", new { gmail = gmail, password = password });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Server error, failed to login {ex}", "Ok");
            return;
        }

        if (!response.IsSuccessStatusCode) { login_errors_label.Text = await response.Content.ReadAsStringAsync();  return; }

        login_errors_label.Text = "logged";
        login_gmail_entry.Text = "";
        login_password_entry.Text = "";

        HttpResponseMessage res = null;
        try
        {
            res = await client.PostAsJsonAsync("Auth/GetUserIdByGmail", new { gmail = gmail });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Server error, failed to login 2 {ex}", "Ok");
            return;
        }

        if (!res.IsSuccessStatusCode) { return; }
        int user_id = await res.Content.ReadFromJsonAsync<int>();
        Application.Current.MainPage = new MainPage(user_id);
    }

    public void Forgot_Password_(object sender, EventArgs e)
    {

    }

    public async void Go_To_Register_(object sender, EventArgs e)
    {
        Application.Current.MainPage = new Register_Page();
    }
}