using AtendeLogo.Application.Models.Communication;

namespace AtendeLogo.Infrastructure.Services;

public class EmailSender : IEmailSender
{
    public async Task<bool> SendEmailAsync(Email email)
    {
        //todo implement email sending
        await Task.Delay(1000);
        return true;
    }
}
