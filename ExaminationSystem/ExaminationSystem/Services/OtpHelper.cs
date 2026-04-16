using System.Text;
using System.Security.Cryptography;


namespace ExaminationSystem.Services
{
    public static class OtpHelper
    {
        public static string GenerateOtp()
        {
            return RandomNumberGenerator.GetInt32(100000, 999999).ToString();
        }
        public static string HashOtp(string otp)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(otp);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
