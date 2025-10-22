using SQLite;

namespace PastryServer.Models
{
    public class Address
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int User_Id { get; set; }
        public string Address_ { get; set; } = "";
    }
}
