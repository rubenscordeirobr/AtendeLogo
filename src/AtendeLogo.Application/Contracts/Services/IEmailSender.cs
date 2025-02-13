using AtendeLogo.Application.Models;

namespace AtendeLogo.Application.Contracts.Services;

public interface IEmailSender
{
    Task<bool> SendEmailAsync(Email email);
}