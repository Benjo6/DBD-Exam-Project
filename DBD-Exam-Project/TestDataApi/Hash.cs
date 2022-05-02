using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace TestDataApi
{
    public class Hash
    {
        public static string Create(string value, string salt)
        {
            //return GetFastMockHash(value, salt);
            var valueBytes = KeyDerivation.Pbkdf2(
                                password: value,
                                salt: Encoding.UTF8.GetBytes(salt),
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 1,
                                numBytesRequested: 64 / 8);

            return Convert.ToBase64String(valueBytes);
        }

private static string GetFastMockHash(string value, string salt)
    {
        string combined = (value ?? "") + (salt ?? "") + "ThIsIsNoTaHaSh";
        try{
        var subStr = combined.Substring(0,8);
        return subStr.Reverse().ToString();
        }
        catch (Exception ex){
            Console.WriteLine($"Failed to generate hash for {value} - {salt}, Combined was: {combined}" );
            Console.WriteLine(ex);
            throw;
        }
    }

    public static bool Validate(string value, string salt, string hash)
            => Create(value, salt) == hash;
    }
}
