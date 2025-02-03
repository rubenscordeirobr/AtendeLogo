namespace AtendeLogo.Common.UnitTests.Utils;

public class PhoneNumberUtilsTest
{
    [Theory]
    [InlineData("+15551234567", CountryCode.USA)]
    [InlineData("+521234567890", CountryCode.MEX)]
    [InlineData("+5511987654321", CountryCode.BRA)]
    public void GetCountryCode_ValidNumber_ReturnsExpectedCountryCode(string fullNumber, CountryCode expectedCountryCode)
    {
        var result = PhoneNumberUtils.GetCountryCode(fullNumber);

        result.Should().Be(expectedCountryCode);
    }

    [Theory]
    [InlineData("+15551234567", InternationalDialingCode.UnitedStatesOrCanada)]
    [InlineData("+521234567890", InternationalDialingCode.Mexico)]
    [InlineData("+5511987654321", InternationalDialingCode.Brazil)]
    public void GetInternationalDialingCode_ValidNumber_ReturnsExpectedDialingCode(string fullNumber, InternationalDialingCode expectedDialingCode)
    {
        var result = PhoneNumberUtils.GetInternationalDialingCode(fullNumber);
        result.Should().Be(expectedDialingCode);
    }

    [Theory]
    [InlineData("+15551234567", true)]
    [InlineData("+521234567890", true)]
    [InlineData("+5511987654321", true)]
    [InlineData("123456", false)]
    [InlineData(null, false)]
    public void IsFullPhoneNumberValid_VariousNumbers_ReturnsExpectedValidity(
        string? fullNumber, bool expectedValidity)
    {
        var result = PhoneNumberUtils.IsFullPhoneNumberValid(fullNumber);
        result.Should().Be(expectedValidity);
    }

    [Theory]
    [InlineData(CountryCode.USA, "5551234567", true)]
    [InlineData(CountryCode.MEX, "1234567890", true)]
    [InlineData(CountryCode.BRA, "11987654321", true)]
    [InlineData(CountryCode.Unknown, "123456", false)]
    public void IsNationalNumberValid_VariousNumbers_ReturnsExpectedValidity(CountryCode countryCode, string nationalNumber, bool expectedValidity)
    {
        var result = PhoneNumberUtils.IsNationalNumberValid(countryCode, nationalNumber);
        result.Should().Be(expectedValidity);
    }
}
