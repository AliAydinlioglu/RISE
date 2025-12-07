using System.Security.Cryptography;
using System.Text;

namespace Rise.Domain.Common;

public static class Hashing
{
    public static string ToMd5String(string value)
    {
        using var md5 = MD5.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(value);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        // Byte[] -> hex string
        return Convert.ToHexString(hashBytes);
    }
}
