namespace RAuth.Application.Util
{
    public class GenerateRAuthSecrets
    {
        public static string GenerateClientSecret()
        {
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            byte[] randomBytes = new byte[32];
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
