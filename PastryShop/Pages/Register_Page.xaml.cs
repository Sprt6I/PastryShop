namespace PastryShop;
using Microsoft.Maui.Controls;
using System.Net.Http.Json;
using PastryServer.Helper_Files;

public partial class Register_Page : ContentPage
{
    private static readonly HttpClient client;

    static Register_Page()
    {
        HttpClientHandler handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        client = new HttpClient(handler) { BaseAddress = new Uri("https://192.168.0.31:5001/") };
    }

    public Register_Page()
	{
		InitializeComponent();
    }

    public async void Register_(object sender, EventArgs e)
    {
        string gmail = register_gmail_entry.Text?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(gmail)) { register_errors_label.Text = "gmail cant be empty"; return; }
        if (!Checks.Is_Gmail_Valid_(gmail)) { register_errors_label.Text = "gmail is invalid"; return; }

        string password = register_password_entry.Text?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(password)) { register_errors_label.Text = "password cant be empty"; return; }
        if (!Checks.Is_Password_Valid_(password)) { register_errors_label.Text = "password is invalid ( must have big and small letter as well as number nad special character)";  return; }

        string repeat_password = register_password_entry_repeat.Text?.Trim() ?? "";
        if (password != repeat_password) { register_errors_label.Text = "passwords do not match"; return; }

        string code = register_code_entry.Text?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(code)) { register_errors_label.Text = "code cant be empty"; return; }

        HttpResponseMessage? response = null;
        try
        {
            response = await client.PostAsJsonAsync("Auth/Register", new { gmail = gmail, password = password, verification_code = code });
            if (response == null) { register_errors_label.Text = "Server error, failed to register"; return; }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Server error, failed to register {ex}", "Ok");
            return;
        }

        register_errors_label.Text = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) { return; }

        register_gmail_entry.Text = "";
        register_password_entry.Text = "";
        if (Application.Current == null) { await DisplayAlert("Error", "Failed To Initialize App", "Ok"); return; }
        Application.Current.MainPage = new Login_Page();
    }


    public async void Send_Register_Confirmation_Code_(object sender, EventArgs e)
    {
        string gmail = register_gmail_entry.Text?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(gmail)) { register_errors_label.Text = "gmail cant be empty"; return; }
        if (!Checks.Is_Gmail_Valid_(gmail)) { register_errors_label.Text = "gmail must be valid"; return; }

        try
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("Auth/SentVerificationGmail", new { gmail = gmail });
            register_errors_label.Text = await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Server error, failed to send code {ex}", "Ok");
            return;
        }
    }
    
    public async void Go_To_Login_(object sender, EventArgs e)
    {
        if (Application.Current == null) { await DisplayAlert("Error", "Failed To Initialize App", "Ok"); return; }
        Application.Current.MainPage = new Login_Page();
    }
}