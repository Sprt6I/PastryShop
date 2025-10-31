using PastryServer.Models;

namespace PastryServer.Requests
{
    public class Product_Group
    {
        public string Category { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = new();
    }
}
