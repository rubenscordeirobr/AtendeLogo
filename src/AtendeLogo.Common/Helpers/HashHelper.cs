
using System.Security.Cryptography;
using System.Text;

namespace AtendeLogo.Common.Helpers;

public class HashHelper
{
    public static string CreateSha256Hash(string input)
    {
        return CreateSha256Hash(Encoding.UTF8.GetBytes(input));
    }

    public static string CreateSha256Hash(Guid firstSystemSession_Id)
    {
        return CreateSha256Hash(firstSystemSession_Id.ToByteArray());
    }

    public static string CreateSha256Hash(byte[] bytes)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(bytes);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}
