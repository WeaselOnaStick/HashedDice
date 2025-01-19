using System.Security.Cryptography;
using System.Text;

class SecureRandomGen
{
    public int secretInt;
    private byte[] hmac;
    private byte[] secretKey;

    public string Hmac { get { return Convert.ToHexString(hmac).ToUpper(); } }
    public string SecretKey { get { return Convert.ToHexString(secretKey).ToUpper(); } }

    public SecureRandomGen(int a, int b)
    {
        this.secretKey = GenerateRandomKey();
        this.secretInt = GenerateRandomInt(a, b);
        this.hmac = GenerateHMAC(secretKey, this.secretInt);

    }

    byte[] GenerateRandomKey()
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] keyBytes = new byte[32];
            rng.GetBytes(keyBytes);
            return keyBytes;
        }
    }

    int GenerateRandomInt(int a, int b)
    {
        Random random = new Random();
        return random.Next(a, b + 1);
    }

    byte[] GenerateHMAC(byte[] key, int message)
    {
        byte[] messageBytes = BitConverter.GetBytes(message);

        using (var hmac = new HMACSHA3_256(key))
            return hmac.ComputeHash(messageBytes);
    }
}
