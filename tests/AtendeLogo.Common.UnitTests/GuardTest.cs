﻿namespace AtendeLogo.Common.UnitTests;

public class GuardTest
{
    [Theory]
    [InlineData(null)]
    public void NotNull_ShouldThrowArgumentNullException_WhenValueIsNull(object? value)
    {
        Action act = () => Guard.NotNull(value);

        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void NotNullOrWhiteSpace_ShouldThrowArgumentException_WhenValueIsNullOrWhiteSpace(string? value)
    {
        Action act = () => Guard.NotNullOrWhiteSpace(value);

        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("123")]
    [InlineData("invalid-phone-number")]
    public void FullPhoneNumber_ShouldThrowArgumentException_WhenValueIsInvalid(string? value)
    {
        Action act = () => Guard.FullPhoneNumber(value);

        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Positive_ShouldThrowArgumentOutOfRangeException_WhenValueIsNotPositive(int value)
    {
        Action act = () => Guard.Positive(value);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("invalid-sha256")]
    public void Sha256_ShouldThrowArgumentException_WhenValueIsNotSha256(string? value)
    {
        Action act = () => Guard.Sha256(value);

        act.Should().Throw<ArgumentException>();
    }
}
