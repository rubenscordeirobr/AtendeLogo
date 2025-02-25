namespace AtendeLogo.UseCases.Identities.Users.TenantUsers.Queries;

public record GetTenantUserByIdQuery(Guid Id) 
    : GetEntityByIdQuery<TenantUserResponse>(Id);
