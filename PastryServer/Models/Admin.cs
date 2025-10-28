using SQLite;

namespace PastryServer.Models
{
    public class Admin
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } = 0;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
