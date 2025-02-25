namespace AtendeLogo.UseCases.Identities.Users.TenantUsers.Queries;

public record GetTenantUserByEmailQuery(
    string Email) 
    : QueryRequest<TenantUserResponse>;
