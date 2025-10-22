namespace PastryShop;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        if (!Is_Gmail_Valid_(gmail)) { login_errors_label.Text = "Gmail must be correct"; return; }

        string password = login_password_entry.Text?.Trim() ?? "";
        login_password_entry.Text = "";
        if (string.IsNullOrWhiteSpace(password)) { login_errors_label.Text = "password can't be empty"; return; }
        if (!Is_Password_Valid_(password)) { login_errors_label.Text = "password must be valid"; return; }
            
        var response = await client.PostAsJsonAsync("Auth/Login", new { gmail = gmail, password = password });

        if (!response.IsSuccessStatusCode) { login_errors_label.Text = await response.Content.ReadAsStringAsync();  return; }

        login_errors_label.Text = "logged";
        login_gmail_entry.Text = "";
        login_password_entry.Text = "";
        // Go_To_Main_App
    }

    public bool Is_Gmail_Valid_(string gmail)
    {
        if (string.IsNullOrWhiteSpace(gmail)) { return false; }

        string pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
        if (!Regex.IsMatch(gmail, pattern, RegexOptions.IgnoreCase)) { return false; }

        return true;
    }

    public bool Is_Password_Valid_(string password)
    {
        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{3,}$";
        return Regex.IsMatch(password, pattern);
    }

    public void Forgot_Password_(object sender, EventArgs e)
    {

    }

    public async void Go_To_Register_(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Register_Page());
    }
}