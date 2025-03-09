namespace AtendeLogo.UseCases.UnitTests.Validations;

public class PhoneNumberValidatorTests : IClassFixture<AnonymousServiceProviderMock>
{
    private readonly IValidator<PhoneNumber> _validator;

    public PhoneNumberValidatorTests(AnonymousServiceProviderMock serviceProviderMock)
    {
        _validator = serviceProviderMock.GetRequiredService<IValidator<PhoneNumber>>();
    }

    [Fact]
    public void Validator_ShouldBePhoneNumberValidatorType()
    {
        _validator.Should().BeOfType<PhoneNumberValidator>();
    }

        [Fact]
    public void When_PhoneNumber_IsEmpty_ValidationFails()
    {
        // Arrange
        var phone = new PhoneNumber(string.Empty);

        // Act
        var result = _validator.TestValidate(phone);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Number);
    }

    [Fact]
    public void When_PhoneNumber_Exceeds_MaximumLength_ValidationFails()
    {
        // Arrange
        int maxLength = ValidationConstants.PhoneNumberMaxLength;
        var tooLongNumber = new string('1', maxLength + 1);
        var phone = new PhoneNumber(tooLongNumber);

        // Act
        var result = _validator.TestValidate(phone);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Number);
    }

    [Fact]
    public void When_PhoneNumber_StartWithPlus_ValidationFails()
    {
        // Arrange
        var validNumber = "11234567890";
        var phone = new PhoneNumber(validNumber);

        // Act
        var result = _validator.TestValidate(phone);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Number);
    }

    [Fact]
    public void When_PhoneNumber_IsValid_ValidationSucceeds()
    {
        // Arrange
        var validNumber = "+11234567890";
        var phone = new PhoneNumber(validNumber);

        // Act
        var result = _validator.Validate(phone);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}

