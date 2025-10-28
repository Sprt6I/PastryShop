namespace PastryAdmin.Pages;

public partial class Admin_Login_Page : ContentPage
{
	public Admin_Login_Page()
	{
		InitializeComponent();
	}

	public void Login_(object sender, EventArgs e)
	{
		string login = login__entry.Text?.Trim() ?? "";
		if (string.IsNullOrWhiteSpace(login)) { return; }

		string password = password__entry.Text?.Trim() ?? "";
		if (string.IsNullOrWhiteSpace(password)) { return; }
	}
}