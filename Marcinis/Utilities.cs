using System.Text;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace Marcinis
{
    public class Utilities
    {
        public static string GeneratePasswordSalt()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(128 / 8));
        }

        public static string GeneratePasswordHash(byte[] salt, string password)
        {
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hash;
        }
    }
}
