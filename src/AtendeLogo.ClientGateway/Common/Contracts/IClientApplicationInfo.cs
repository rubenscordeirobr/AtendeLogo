namespace AtendeLogo.ClientGateway.Common.Contracts;

public interface IClientApplicationInfo
{
    string ApplicationName { get; }
    Version ApplicationVersion { get; }
    string Environment { get; }
}
