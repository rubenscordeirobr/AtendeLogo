namespace AtendeLogo.Application.Contracts.Services;

public interface IEntityAuthorizationService : IApplicationService
{
    /// <exception cref="UnauthorizedSecurityException"></exception>
    void ValidateEntityChange(
         EntityBase entity,
         IUserSession userSession,
         EntityChangeState entityChangeState);
}
