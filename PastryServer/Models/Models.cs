namespace PastryServer
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
}
