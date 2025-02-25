namespace AtendeLogo.UseCases.Identities.Users.TenantUsers.Queries;

public record GetTenantUserByPhoneNumberQuery(
    string PhoneNumber)
    : QueryRequest<TenantUserResponse>;
