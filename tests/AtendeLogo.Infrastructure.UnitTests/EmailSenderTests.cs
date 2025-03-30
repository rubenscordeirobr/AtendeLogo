using AtendeLogo.Application.Models.Communication;
using AtendeLogo.Infrastructure.Services;

namespace AtendeLogo.Infrastructure.UnitTests;

//Placeholder for tests
public class EmailSenderTests
{
    [Fact]
    public async Task Test1()
    {
        //Arrange
        var email = new Email {
            Body = "Test Body",
            From = "Test From",
            Subject = "Test Subject",
            To = "teste@test.com"
        };
         
        var emailSender = new EmailSender();

        //Act
        var result = await emailSender.SendEmailAsync(email);

        //Assert
        result.Should().BeTrue();
    }
}
