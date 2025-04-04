using AtendeLogo.Application.Models.Communication;

namespace AtendeLogo.Application.Abstractions.Services;

public interface IEmailSender : IApplicationService
{
    Task<bool> SendEmailAsync(Email email);
}
