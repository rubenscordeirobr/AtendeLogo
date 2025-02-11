using System.Security.Cryptography;
using System.Text;

namespace AtendeLogo.Common.Helpers;

public class HashHelper
{
    public static string GenerateSha256HashFromString(string input)
    {
        return GenerateSha256Hash(Encoding.UTF8.GetBytes(input));
    }

    public static string CreateSha256Hash(Guid firstSystemSession_Id)
    {
        return GenerateSha256Hash(firstSystemSession_Id.ToByteArray());
    }

    public static string GenerateSha256Hash(byte[] bytes)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(bytes);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    public static Guid CreateMd5GuidHash(string input)
    {
        return GenerateMd5GuidHash(Encoding.UTF8.GetBytes(input));
    }

    public static Guid GenerateMd5GuidHash(byte[] bytes)
    {
        using var md5 = MD5.Create();
        var hashBytes = md5.ComputeHash(bytes);
        return new Guid(hashBytes);
    }
}
