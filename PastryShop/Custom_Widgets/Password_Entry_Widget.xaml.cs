namespace PastryShop.Custom_Widgets;

public partial class Password_Entry_Widget : ContentView
{
	bool is_password = true;
	public Password_Entry_Widget()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty TextProperty =
		BindableProperty.Create(
			nameof(Text),
			typeof(string),
			typeof(Password_Entry_Widget),
			string.Empty,
			BindingMode.TwoWay,
			propertyChanged: (bindable, old_value, new_value) =>
			{
				var control = (Password_Entry_Widget)bindable;
				control.password_entry.Text = (string)new_value;
			}
		);

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	private void Show_Password_(object sender, EventArgs e)
	{
		is_password = !is_password;
		password_entry.IsPassword = is_password;
		if (is_password) { show_password_button.Text = "😮"; }
		else { show_password_button.Text = "😑"; }
    }
}