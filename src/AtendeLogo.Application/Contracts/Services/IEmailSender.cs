using AtendeLogo.Application.Models.Communication;

namespace AtendeLogo.Application.Contracts.Services;

public interface IEmailSender
{
    Task<bool> SendEmailAsync(Email email);
}