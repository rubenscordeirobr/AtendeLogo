namespace AtendeLogo.UseCases.Identities.Users.AdminUsers.Queries;

public record GetAdminUserByEmailOrPhoneNumberQuery(
    string EmailOrPhonenumber )
    : QueryRequest<AdminUserResponse>;
