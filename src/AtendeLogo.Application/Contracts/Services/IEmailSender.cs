using AtendeLogo.Application.Models.Communication;

namespace AtendeLogo.Application.Contracts.Services;

public interface IEmailSender : IApplicationService
{
    Task<bool> SendEmailAsync(Email email);
}
