using SQLite; 

namespace PastryServer.Models
{
    public class User_Order
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int User_Id { get; set; } = 0;
        string Order_Identifier { get; set; } = "";
        public List<Product> Products { get; set; } = new();
    }
}
