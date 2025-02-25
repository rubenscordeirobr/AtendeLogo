namespace AtendeLogo.UseCases.Identities.Users.TenantUsers.Queries;

public record GetTenantUserByEmailOrPhoneNumberQuery(
    string Email, 
    string PhoneNumber) 
    : QueryRequest<TenantUserResponse>;
