using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace JobPortal_New.HelperMethods
{
    public class PasswordHasher
    {
        [NonAction]
        public static string HashPassword(string? password)
        {
            byte[] salt = Encoding.UTF8.GetBytes("fixedSaltValue123");

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);

            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
        }
    }
}
