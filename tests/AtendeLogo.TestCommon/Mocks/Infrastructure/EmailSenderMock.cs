using AtendeLogo.Application.Models.Communication;

namespace AtendeLogo.TestCommon.Mocks.Infrastructure;

internal class EmailSenderMock : IEmailSender
{
    public Task<bool> SendEmailAsync(Email email)
    {
        return Task.FromResult(true);
    }
}
