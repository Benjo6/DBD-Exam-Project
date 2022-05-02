using System.Security.Cryptography;

namespace TestDataApi
{
    public class Salt
    {
        public static string Create()
        {
            byte[] randomBytes = new byte[32 / 8];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
    }
}
