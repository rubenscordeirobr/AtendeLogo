﻿using AtendeLogo.Shared.Contantes;
using AtendeLogo.Shared.Enums;
using AtendeLogo.Domain.Extensions;
using AtendeLogo.Shared.Interfaces.Identities;
using Moq;

namespace AtendeLogo.Application.UnitTests.Domain.Extensions;

public class UserSessionContextExtensionsTests
{
    [Fact]
    public void IsAnonymous_ShouldReturnTrue_WhenUserIsAnonymous()
    {
        // Arrange
        var userSession = new Mock<IUserSession>();
        userSession.Setup(us => us.User_Id).Returns(AnonymousConstants.AnonymousUser_Id);
        userSession.Setup(us => us.AuthenticationType)
            .Returns(AuthenticationType.Anonymous);

        // Act
        var result = userSession.Object.IsAnonymous();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsAnonymous_ShouldReturnFalse_WhenUserIsNotAnonymous()
    {
        // Arrange
        var userSession = new Mock<IUserSession>();
        userSession.Setup(us => us.User_Id).Returns(Guid.NewGuid());
        userSession.Setup(us => us.AuthenticationType).Returns(AuthenticationType.Email_Password);

        // Act
        var result = userSession.Object.IsAnonymous();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsTenantUser_ShouldReturnTrue_WhenTenantIdIsValid()
    {
        // Arrange
        var userSession = new Mock<IUserSession>();
        userSession.Setup(us => us.Tenant_Id).Returns(Guid.NewGuid());

        // Act
        var result = userSession.Object.IsTenantUser();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsTenantUser_ShouldReturnFalse_WhenTenantIdIsNull()
    {
        // Arrange
        var userSession = new Mock<IUserSession>();
        userSession.Setup(us => us.Tenant_Id).Returns((Guid?)null);

        // Act
        var result = userSession.Object.IsTenantUser();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsTenantUser_ShouldReturnFalse_WhenTenantIdIsEmpty()
    {
        // Arrange
        var userSession = new Mock<IUserSession>();
        userSession.Setup(us => us.Tenant_Id).Returns(Guid.Empty);

        // Act
        var result = userSession.Object.IsTenantUser();

        // Assert
        result.Should().BeFalse();
    }
}
