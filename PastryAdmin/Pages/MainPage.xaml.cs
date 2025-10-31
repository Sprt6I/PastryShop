using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using PastryServer.Models;
using System.Runtime.InteropServices;
using System.Net.Http.Json;

namespace PastryAdmin
{
    public partial class MainPage : ContentPage
    {
        public ISeries[] series { get; set; }
        public Axis[] x_axes { get; set; }
        public Axis[] y_axes { get; set; }

        private static readonly HttpClient client;

        static MainPage()
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            client = new HttpClient(handler) { BaseAddress = new Uri("https://192.168.0.31:5001/") };
        }

        public MainPage()
        {
            InitializeComponent();
            Initialize_Chart_();
        }

        private async Task Initialize_Chart_()
        {
            Create_Users_Chart(await Get_Users_());
        }
        public async Task<List<User>> Get_Users_()
        {
            try
            {
                List<User> users = await client.GetFromJsonAsync<List<User>>("Admin/GetAllUsers");
                return users;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Server Error, Failed to load Users. {ex}", "OK");
                return new List<User>();
            }
        }
        public void Create_Users_Chart(List<User> users)
        {
            var today = DateTime.Now.Date;
            // Group by hour for users registered today
            var grouped = users
                .Where(u => u.Registration_Time_And_Date.Date == today)
                .GroupBy(u => u.Registration_Time_And_Date.Hour)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.Count());

            var hours = Enumerable.Range(0, 24).ToArray();
            var counts = hours.Select(h => grouped.ContainsKey(h) ? grouped[h] : 0).ToArray();

            series = new ISeries[]
            {
                new ColumnSeries<int>
                {
                    Values = counts,
                    Fill = new SolidColorPaint(SKColors.DodgerBlue),
                    Name = "User Registrations"
                }
            };

            x_axes = new Axis[]
                {
                  new Axis
                    {
                    Labels = hours.Select(h => $"{h}:00").ToArray(),
                    LabelsRotation = 45,
                    Name = "Hour"
                    }
                };

            y_axes = new Axis[]
            {
                new Axis
                {
                    Name = "Number of Users",
                    Labeler = value => value.ToString("N0")
                }
            };
        }

        private async void Go_To_Product_Manager_(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Products_Manager_Page());
        }

        private async void Refresh_Chart_(object sender, EventArgs e)
        {
            await Initialize_Chart_();
        }
    }

}
