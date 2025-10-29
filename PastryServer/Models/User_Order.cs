using SQLite;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace PastryServer.Models
{
    public class User_Order
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int User_Id { get; set; } = 0;
        string Order_Identifier { get; set; } = "";
        public string ProductsJson { get; set; } = "[]";

        [Ignore]
        public List<Product> Products
        {
            get => JsonSerializer.Deserialize<List<Product>>(ProductsJson) ?? new();
            set => ProductsJson = JsonSerializer.Serialize(value);
        }
    }
}
