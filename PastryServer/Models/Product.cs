using SQLite;

namespace PastryServer.Models
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public float Price { get; set; } = 0;
        public int In_Stock { get; set; } = 0;
    }
}
