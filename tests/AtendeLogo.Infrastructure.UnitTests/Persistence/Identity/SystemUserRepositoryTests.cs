﻿using AtendeLogo.Application.Abstractions.Persistence.Identities;

namespace AtendeLogo.Infrastructure.UnitTests.Persistence.Identity;

public class SystemUserRepositoryTests: IClassFixture<ServiceProviderMock<AnonymousRole>>
{
    private readonly IServiceProvider _serviceProvider;

    public SystemUserRepositoryTests(
        ServiceProviderMock<AnonymousRole> serviceProvider,
        ITestOutputHelper testOutput)
    {
        serviceProvider.AddTestOutput(testOutput);

        _serviceProvider = serviceProvider;
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnNonEmptyList()
    {
        // Arrange
        var systemUserRepository = _serviceProvider.GetRequiredService<ISystemUserRepository>();

        // Act
        var users = await systemUserRepository.GetAllAsync();

        // Assert
        systemUserRepository.Should()
            .NotBeNull();

        users.Should()
            .NotBeNull()
            .And
            .NotBeEmpty();
    }

    [Fact]
    public async Task GetById_ShouldReturnAnonymousUser()
    {
        // Arrange
        var systemUserRepository = _serviceProvider.GetRequiredService<ISystemUserRepository>();
        var anonymousId = AnonymousUserConstants.User_Id;
      
        // Act
        var anonymousUser = await systemUserRepository.GetByIdAsync(anonymousId);
       
        // Assert
        anonymousUser.Should()
            .NotBeNull();

        anonymousUser!.Id.Should()
            .Be(anonymousId);
    }
}

