namespace AtendeLogo.Common.Utils;

public static class JwtUtils
{
    public const string AuthenticationScheme = "Bearer";
    public static string? GetTokenFromAuthorizationHeader(string? authorizationHeader)
    {

        if (string.IsNullOrWhiteSpace(authorizationHeader))
        {
            return null;
        }
   
        if (authorizationHeader.StartsWith(AuthenticationScheme, StringComparison.Ordinal))
        {
            return authorizationHeader.Substring(AuthenticationScheme.Length).Trim();
        }
        return null;
    }

    public static string FormatAsAuthorizationHeader(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return string.Empty;
        }
        if(token.StartsWith(AuthenticationScheme, StringComparison.Ordinal))
        {
            return token;
        }
        return $"{AuthenticationScheme} {token}";
    }
}

