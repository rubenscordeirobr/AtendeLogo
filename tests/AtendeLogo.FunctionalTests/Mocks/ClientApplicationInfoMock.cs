namespace AtendeLogo.FunctionalTests.Mocks;

public class ClientApplicationInfoMock : IClientApplicationInfo
{
    public string ApplicationName 
        => "AtendeLogo.Test";

    public Version ApplicationVersion 
        => new Version(1, 0, 0);

    public string Environment
        => "Test";
}
