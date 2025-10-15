using SQLite;

namespace PastryServer
{
    public class Verification_Code
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Gmail { get; set; } = "";
        public string Code { get; set; } = "";
        public DateTime Expires_At { get; set; }
    }
}
