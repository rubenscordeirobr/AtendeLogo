using AtendeLogo.Application.Models.Security;
namespace AtendeLogo.Application.Contracts.Security;

public interface IAuthenticationAttemptLimiterService
{
    Task IncrementFailedAttemptsAsync(string ipAddress);
    Task<MaxAuthenticationResult> MaxAuthenticationReachedAsync(string ipAddress);
}
