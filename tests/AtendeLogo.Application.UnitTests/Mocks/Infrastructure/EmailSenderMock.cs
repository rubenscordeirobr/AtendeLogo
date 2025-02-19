using AtendeLogo.Application.Models.Communication;

namespace AtendeLogo.Application.UnitTests.Mocks.Infrastructure;

internal class EmailSenderMock : IEmailSender
{
    public Task<bool> SendEmailAsync(Email email)
    {
        return Task.FromResult(true);
    }
}
