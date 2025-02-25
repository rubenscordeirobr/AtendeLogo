namespace AtendeLogo.UseCases.Identities.Users.AdminUsers.Queries;

public record GetAdminUserByEmailOrPhoneNumberQuery(
    string Email,
    string PhoneNumber)
    : QueryRequest<AdminUserResponse>;
