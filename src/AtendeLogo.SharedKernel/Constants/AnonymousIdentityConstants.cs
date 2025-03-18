using AtendeLogo.Shared.Interfaces.Identities;
using AtendeLogo.Shared.Models.Identities;

namespace AtendeLogo.Shared.Constants;

public static class AnonymousIdentityConstants
{
    public const string Email = "anonymous@atendelogo.com";
    public const string Name = "Anonymous";

    public static readonly Guid Session_Id = new("49AD1FD6-0385-40DF-9F0F-538B61065442");
    public static readonly Guid User_Id = new("FB8A6865-A999-0000-8BB7-F4D70ACF1232");

#pragma warning disable CS9264 

    public static string ClientSystemSessionToken
        => field ??= HashHelper.CreateSha256Hash(Session_Id);

    public static IUser AnonymousUser
        => field ??= new AnonymousUser();

#pragma warning restore CS9264
}
