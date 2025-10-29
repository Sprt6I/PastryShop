using SQLite;
using System.ComponentModel.DataAnnotations;

namespace PastryServer.Models
{
    public class User_Cart
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } = 0;
        
        [Required]
        public int User_Id { get; set; } = 0;

        [Required]
        public List<Product> Products { get; set; } = new();
    }
}
