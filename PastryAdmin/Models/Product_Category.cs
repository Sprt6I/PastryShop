using SQLite;

namespace PastryServer.Models
{
    public class Product_Category
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
