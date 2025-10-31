using SQLite;
using System.Collections.Generic;
using PastryServer.Models;
using System.Text.Json;

namespace PastryServer.Models
{
    public class Bought_Product
    {
        public int Product_Id { get; set; } = 0;
        public int Quantity { get; set; } = 0;
    }

        public class User_Cart
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } = 0;

        public int User_Id { get; set; } = 0;

        public string Bought_Products_Json { get; set; } = "[]";

        [Ignore]
        public List<Bought_Product> Bought_Products
        {
            get => JsonSerializer.Deserialize<List<Bought_Product>>(Bought_Products_Json) ?? new();
            set => Bought_Products_Json = JsonSerializer.Serialize(value);
        }
    }
}
