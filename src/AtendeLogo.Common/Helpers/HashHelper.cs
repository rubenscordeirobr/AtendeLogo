using System.Diagnostics.CodeAnalysis;
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
        var hashBytes = SHA256.HashData(bytes);
        return Convert.ToHexStringLower(hashBytes);
    }

    public static Guid CreateMd5GuidHash(string input)
    {
        return GenerateMd5GuidHash(Encoding.UTF8.GetBytes(input));
    }

    
    public static Guid GenerateMd5GuidHash(byte[] bytes)
    {
        var hashBytes = MD5.HashData(bytes);
        return new Guid(hashBytes);
    }
}
