namespace PastryServer.Requests
{
    public class Add_To_Cart_Request
    {
        public int User_Id { get; set; } = 0;
        public int Product_Id { get; set; } = 0;
        public int Product_Quantity { get; set; } = 0;
    }
}
