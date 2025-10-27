using System.Text.RegularExpressions;

namespace PastryServer.Helper_Files
{
    public class Checks
    {
        public bool Is_Gmail_Valid_(string gmail)
        {
            if (string.IsNullOrWhiteSpace(gmail)) { return false; }

            string pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
            if (!Regex.IsMatch(gmail, pattern, RegexOptions.IgnoreCase)) { return false; }

            return true;
        }

        public bool Is_Password_Valid_(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{3,}$";
            return Regex.IsMatch(password, pattern);
        }
    }
}
