using SQLite;

namespace PastryServer
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public string Gmail { get; set; } = "";

        public string Password { get; set; } = "";

        public bool verified_gmail { get; set; } = false;

        public int address_id { get; set; } // user can have multiple adresses so there will be separate tabnle with addresses and will show only with user_id for that user

    }
}
