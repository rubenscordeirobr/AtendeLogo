﻿using AtendeLogo.Application.Contracts.Security;
using AtendeLogo.Common.Helpers;
using AtendeLogo.UseCases.Contracts.Identities;
using AtendeLogo.UseCases.Identities.Authentications.Services;
using Moq;

namespace AtendeLogo.UseCases.UnitTests.Identities.Authentications.Services;

public class TenantAuthenticationValidationServiceTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<ITenantUserRepository> _tenantUserRepositoryMock;
    private readonly Mock<ISecureConfiguration> _secureConfigurationMock;
    private readonly TenantAuthenticationValidationService _validationService;
    private readonly CancellationToken _token = CancellationToken.None;

    public TenantAuthenticationValidationServiceTests()
    {
        _tenantUserRepositoryMock = new Mock<ITenantUserRepository>();
        _secureConfigurationMock = new Mock<ISecureConfiguration>();

        _validationService = new TenantAuthenticationValidationService(
            _tenantUserRepositoryMock.Object,
            _secureConfigurationMock.Object);
    }

    [Fact]
    public void Service_ShouldBeRegistered()
    {
        // Arrange
        var anonymousServiceProviderMock = new AnonymousServiceProviderMock();
        // Act
        var service = anonymousServiceProviderMock.GetRequiredService<ITenantAuthenticationValidationService>();
        // Assert
        service.Should().BeOfType<TenantAuthenticationValidationService>();
    }

    [Fact]
    public async Task VerifyTenantUserCredentialsAsync_UserNotFound_ReturnsFalse()
    {
        // Arrange
        var emailOrPhone = "test@example.com";
        var password = "anyPassword";
        _tenantUserRepositoryMock
            .Setup(repo => repo.GetByEmailOrPhoneNumberAsync(emailOrPhone, _token))
            .ReturnsAsync((TenantUser)null!);

        // Act
        var result = await _validationService.VerifyTenantUserCredentialsAsync(emailOrPhone, password, _token);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task VerifyTenantUserCredentialsAsync_ValidCredentials_ReturnsTrue()
    {
        // Arrange
        var emailOrPhone = "test@example.com";
        var password = "password";
        var salt = "mysalt";
        
        _secureConfigurationMock.Setup(cfg => cfg.GetPasswordSalt()).Returns(salt);
        
        var expectedHash = PasswordHelper.HashPassword(password, salt);
        var passwordInstance = Password.Create(password, salt).Value!;
        var tenantUser = CreateTenantUser(passwordInstance);

        _tenantUserRepositoryMock
            .Setup(repo => repo.GetByEmailOrPhoneNumberAsync(emailOrPhone, _token))
            .ReturnsAsync(tenantUser);

        // Act
        var result = await _validationService.VerifyTenantUserCredentialsAsync(
                emailOrPhone,
                password,
                _token);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task VerifyTenantUserCredentialsAsync_InvalidCredentials_ReturnsFalse()
    {
        // Arrange
        var emailOrPhone = "test@example.com";
        var password = "wrongPassword";
        var salt = "mysalt";
        _secureConfigurationMock.Setup(cfg => cfg.GetPasswordSalt()).Returns(salt);
        
        var expectedHash = PasswordHelper.HashPassword("password", salt);
       
        var passwordInstance = Password.Create("password", salt).Value!;
        var tenantUser = CreateTenantUser(passwordInstance);

        _tenantUserRepositoryMock
            .Setup(repo => repo.GetByEmailOrPhoneNumberAsync(emailOrPhone, _token))
            .ReturnsAsync(tenantUser);

        // Act
        var result = await _validationService.VerifyTenantUserCredentialsAsync(emailOrPhone, password, _token);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task EmailOrPhoneNumberExitsAsync_Exists_ReturnsTrue()
    {
        // Arrange
        var emailOrPhone = "test@example.com";
        _tenantUserRepositoryMock
            .Setup(repo => repo.EmailOrPhoneNumberExits(emailOrPhone, _token))
            .ReturnsAsync(true);

        // Act
        var result = await _validationService.EmailOrPhoneNumberExitsAsync(emailOrPhone, _token);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task EmailOrPhoneNumberExitsAsync_NotExists_ReturnsFalse()
    {
        // Arrange
        var emailOrPhone = "test@example.com";
        _tenantUserRepositoryMock
            .Setup(repo => repo.EmailOrPhoneNumberExits(emailOrPhone, _token))
            .ReturnsAsync(false);

        // Act
        var result = await _validationService.EmailOrPhoneNumberExitsAsync(emailOrPhone, _token);

        // Assert
        result.Should().BeFalse();
    }

    private TenantUser CreateTenantUser(Password password)
    {
        var tenant = _fixture.Create<Tenant>(); 
        return new TenantUser(
            tenant: tenant,
            name: "Test User",
            email: "test@example.com",
            language: Language.Default,
            role: UserRole.None,
            userState: UserState.New,
            userStatus: UserStatus.Online,
            phoneNumber: new PhoneNumber("+5542999990000")!,
            password: password
        );
    }
}
