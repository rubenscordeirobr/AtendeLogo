namespace AtendeLogo.UseCases.Identities.Users.AdminUsers.Queries;

public record GetAdminUserByEmailQuery(
    string Email)
    : QueryRequest<AdminUserResponse>;
