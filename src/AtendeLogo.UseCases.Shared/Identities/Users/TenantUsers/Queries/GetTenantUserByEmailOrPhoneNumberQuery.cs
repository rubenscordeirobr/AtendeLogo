namespace AtendeLogo.UseCases.Identities.Users.TenantUsers.Queries;

public record GetTenantUserByEmailOrPhoneNumberQuery(
    string EmailOrPhoneNumber) 
    : QueryRequest<UserResponse>;
