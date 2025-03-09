using AtendeLogo.UseCases.Identities.Authentications.Commands;

namespace AtendeLogo.UseCases.UnitTests.Identities.Authentications.Commands;

public class TenantUserLogoutCommandValidatorTests : IClassFixture<AnonymousServiceProviderMock>
{
    private readonly IValidator<TenantUserLogoutCommand> _validator;
    private readonly TenantUserLogoutCommand _validCommand;

    public TenantUserLogoutCommandValidatorTests(AnonymousServiceProviderMock serviceProvider)
    {
        _validator = serviceProvider.GetRequiredService<IValidator<TenantUserLogoutCommand>>();
        _validCommand = new TenantUserLogoutCommand
        {
            ClientRequestId = Guid.NewGuid(),
            ClientSessionToken = "ValidAuthToken"
        };
    }

    [Fact]
    public void Validator_ShouldBe_TenantUserLogoutCommandValidator()
    {
        _validator.Should().BeOfType<TenantUserLogoutCommandValidator>();
    }

    [Fact]
    public async Task ValidationResult_ShouldBeValid()
    {
        // Act
        var result = await _validator.TestValidateAsync(_validCommand);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task ValidationResult_ShouldHaveError_When_ClientSessionToken_IsEmpty()
    {
        // Arrange
        var command = _validCommand with { ClientSessionToken = string.Empty };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ClientSessionToken);
    }

    [Fact]
    public async Task ValidationResult_ShouldHaveError_When_ClientSessionToken_ExceedsMaxLength()
    {
        // Arrange
        var tooLongToken = new string('A', ValidationConstants.AuthTokenMaxLength + 1);
        var command = _validCommand with { ClientSessionToken = tooLongToken };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ClientSessionToken);
    }
}

