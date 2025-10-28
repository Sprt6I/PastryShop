namespace PastryAdmin
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Go_To_Product_Manager_(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Products_Manager_Page());
        }
    }

}
