﻿using AtendeLogo.UseCases.Identities.Tenants.Commands;

namespace AtendeLogo.UseCases.UnitTests.Identities.Tenants.Commands;

public class DeleteTenantCommandHandlerTests : IClassFixture<ServiceProviderMock<AdminUserRole>>
{
    private readonly ITestOutputHelper _testOutput;
    private readonly IRequestMediator _mediator;
    private readonly DeleteTenantCommand _validadeCommand;

    public DeleteTenantCommandHandlerTests(
        ServiceProviderMock<AdminUserRole> serviceProviderMock,
        ITestOutputHelper testOutput)
    {
        serviceProviderMock.AddTestOutput(testOutput);

        _testOutput = testOutput;
        _mediator = serviceProviderMock.GetRequiredService<IRequestMediator>();
        _validadeCommand = new DeleteTenantCommand(SystemTenantConstants.Tenant_Id);
    }

    [Fact]
    public void Handler_ShouldBe_DeleteTenantCommandHandler()
    {
        // Act
        var mediatorTest = (IRequestMediatorTest)_mediator;
        var handlerType = mediatorTest.GetRequestHandler(_validadeCommand);

        // Assert
        handlerType.Should().BeOfType<DeleteTenantCommandHandler>();
    }

    [Fact]
    public async Task HandleAsync_ReturnSuccess()
    {
        //// Arrange
        var serviceProvideMock = new ServiceProviderMock<AdminUserRole>();
        serviceProvideMock.AddTestOutput(_testOutput);

        var mediator = serviceProvideMock.GetRequiredService<IRequestMediator>();

        var fakePhoneNumber = BrazilianFakeUtils.GenerateFakePhoneNumber();
        var fakeEmail = FakeUtils.GenerateFakeEmail();
        var fakeCpf = BrazilianFakeUtils.GenerateCpf();
        var createCommand = new CreateTenantCommand
        {
            Name = "Tenant name",
            FiscalCode = new FiscalCode(fakeCpf),
            TenantName = "Tenant name",
            Email = fakeEmail,
            Password = "Password123!",
            Country = Country.Brazil,
            Language = Language.Portuguese,
            Currency = Currency.BRL,
            BusinessType = BusinessType.CivilRegistryOffice,
            TenantType = TenantType.Company,
            PhoneNumber = new PhoneNumber(fakePhoneNumber)
        };

        var createTenantResult = await mediator.RunAsync(createCommand, CancellationToken.None);

        createTenantResult.IsSuccess
            .Should()
            .BeTrue("The tenant should be created successfully");

        var deleteCommand = new DeleteTenantCommand(createTenantResult.Value!.Id);

        //Act
        var result = await mediator.RunAsync(deleteCommand, CancellationToken.None);

        //// Assert
        result.ShouldBeSuccessful();
    }

    [Fact]
    public async Task HandleAsync_ReturnNotFoundFailure()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var command = new DeleteTenantCommand(tenantId);

        // Act
        var result = await _mediator.RunAsync(command, CancellationToken.None);

        // Assert
        result.ShouldBeFailure<NotFoundError>();
    }

    [Fact]
    public async Task HandleAsync_ReturnNotUnauthorizedFailure()
    {
        // Arrange
        var anonymousServiceProvider = new ServiceProviderMock<AnonymousRole>();
        anonymousServiceProvider.AddTestOutput(_testOutput);
        var mediator = anonymousServiceProvider.GetRequiredService<IRequestMediator>();

        // Act
        var result = await mediator.RunAsync(_validadeCommand, CancellationToken.None);

        // Assert
        result.ShouldBeFailure<ForbiddenError>();
    }
}
 
