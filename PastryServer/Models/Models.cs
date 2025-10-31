namespace PastryServer.Models
{
    public class Gmail_Request
    {
        public string gmail { get; set; } = string.Empty;
    }

    public class  Register_Request
    {
        public string gmail { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string verification_code { get; set; } = string.Empty;
    }

    public class Login_Request
    {
        public string gmail { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }

    public class Product_Group
    {
        public string Category { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = new();
    }
    public class GmailRequest
    {
        public string gmail { get; set; } = string.Empty;
    }

    public class User_Id__Request
    {
        public int user_Id { get; set; } = -1;
    }

    public class Add_To_Cart_Request
    {
        public int User_Id { get; set; } = 0;
        public int Product_Id { get; set; } = 0;
        public int Product_Quantity { get; set; } = 0;
    }
}
