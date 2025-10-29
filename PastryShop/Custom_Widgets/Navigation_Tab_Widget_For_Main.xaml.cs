using PastryShop.Pages;

namespace PastryShop.Custom_Widgets;

public partial class Navigation_Tab_Widget_For_Main : ContentView
{
    public static readonly BindableProperty user_idProperty =
        BindableProperty.Create(nameof(user_id), typeof(int), typeof(Navigation_Tab_Widget_For_Main), 0);

    public int user_id
    {
        get => (int)GetValue(user_idProperty);
        set => SetValue(user_idProperty, value);
    }

    public Navigation_Tab_Widget_For_Main()
    {
        InitializeComponent();
    }

    public async void Go_To_Main_(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage(user_id));
    }

    public async void Go_To_Cart_(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Cart_Page(user_id));
    }

    public async void Go_To_Orders_(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Orders_Page(user_id));
    }
}
