using SHA3.Net;
using System.Text;

namespace Task4.Library
{
    public class Hasher
    {
        public static string ToSHA3256String(string s)
        {
            Sha3 hasher = Sha3.Sha3256();
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            return Convert.ToHexString(hasher.ComputeHash(bytes));
        }
    }
}
