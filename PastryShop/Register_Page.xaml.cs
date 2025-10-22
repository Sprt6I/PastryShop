namespace PastryShop;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public partial class Register_Page : ContentPage
{
    private HttpClient client;
    public Register_Page()
	{
		InitializeComponent();

        client = new HttpClient { BaseAddress = new Uri("https://localhost:5201/") };
    }

    public async void Register_(object sender, EventArgs e)
    {
        string gmail = register_gmail_entry.Text?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(gmail)) { register_errors_label.Text = "gmail cant be empty"; return; }
        if (!Is_Gmail_Valid_(gmail)) { register_errors_label.Text = "gmail is invalid"; return; }

        string password = register_password_entry.Text?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(password)) { register_errors_label.Text = "password cant be empty"; return; }
        if (!Is_Password_Valid_(password)) { register_errors_label.Text = "password is invalid ( must have big and small letter as well as number nad special character)";  return; }

        string code = register_code_entry.Text?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(code)) { register_errors_label.Text = "code cant be empty"; return; }

        var response = await client.PostAsJsonAsync("Auth/Register", new { gmail = gmail, password = password, verification_code = code });

        register_errors_label.Text = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) { return; }

        register_gmail_entry.Text = "";
        register_password_entry.Text = "";
        Go_To_Login_(null, null);
    }


    public async void Send_Register_Confirmation_Code_(object sender, EventArgs e)
    {
        string gmail = register_gmail_entry.Text?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(gmail)) { register_errors_label.Text = "gmail cant be empty"; return; }
        if (!Is_Gmail_Valid_(gmail)) { register_errors_label.Text = "gmail must be valid"; return; }

        var response = await client.PostAsJsonAsync("Auth/SentVerificationGmail", new { gmail = gmail });

        register_errors_label.Text = await response.Content.ReadAsStringAsync();
    }

    public async void Go_To_Login_(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Login_Page());
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
}