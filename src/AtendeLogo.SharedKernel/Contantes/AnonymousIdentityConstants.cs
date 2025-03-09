using AtendeLogo.Shared.Interfaces.Identities;
using AtendeLogo.Shared.Models.Identities;

namespace AtendeLogo.Shared.Contantes;

public static class AnonymousIdentityConstants
{
    public const string AnonymousEmail = "anonymous@atendelogo.com";
    public const string AnonymousName = "Anonymous";

    public static readonly Guid AnonymousSystemSession_Id = new Guid("49AD1FD6-0385-40DF-9F0F-538B61065442");
    public static readonly Guid AnonymousUser_Id = new Guid("FB8A6865-A999-0000-8BB7-F4D70ACF1232");

#pragma warning disable CS9264 

    public static string ClientAnonymousSystemSessionToken
        => field ??= HashHelper.CreateSha256Hash(AnonymousSystemSession_Id);

    public static IUser AnonymousUser
        => field ??= new AnonymousUser();

#pragma warning restore CS9264
}
