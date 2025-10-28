using SQLite;

namespace PastryServer.Models
{
    public class Product_Category
    {
        [PrimaryKey]
        public string Name { get; set; } = string.Empty;
    }
}
