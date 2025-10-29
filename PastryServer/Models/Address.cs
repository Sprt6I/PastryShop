using SQLite;
using System.ComponentModel.DataAnnotations;

namespace PastryServer.Models
{
    public class Address
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int User_Id { get; set; } = 0;
        public string Address_ { get; set; } = "";
    }
}
