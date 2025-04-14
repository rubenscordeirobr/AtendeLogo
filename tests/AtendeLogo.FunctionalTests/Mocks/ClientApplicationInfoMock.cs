using AtendeLogo.Shared.Abstractions;

namespace AtendeLogo.FunctionalTests.Mocks;

public class ClientApplicationInfoMock : IApplicationInfo
{
    public string ApplicationName 
        => "AtendeLogo.Test";

    public Version ApplicationVersion 
        => new Version(1, 0, 0);

    public string Environment
        => "Test";

    public string Title 
        => "AtendeLogo Test Application";
}
