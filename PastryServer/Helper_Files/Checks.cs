using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace PastryServer.Helper_Files
{
    public static class Checks
    {
        public static bool Is_Gmail_Valid_(string gmail)
        {
            if (string.IsNullOrWhiteSpace(gmail)) { return false; }

            string pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
            if (!Regex.IsMatch(gmail, pattern, RegexOptions.IgnoreCase)) { return false; }

            return true;
        }

        public static bool Is_Password_Valid_(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{3,}$";
            return Regex.IsMatch(password, pattern);
        }

        public static string Get_Ipv4_()
        {

            IPEndPoint endPoint;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                endPoint = socket.LocalEndPoint as IPEndPoint ?? null!;
                if (endPoint != null) { return endPoint.Address.ToString(); }
            }

            string ip = Dns.GetHostEntry(Dns.GetHostName())
                .AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork)?
                .ToString() ?? "";
            if (string.IsNullOrEmpty(ip))
            {
                throw new Exception("No network adapters with an IPv4 address in the system!");
            }

            return ip;
        }
    }
}
