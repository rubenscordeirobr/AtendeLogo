namespace AtendeLogo.ClientGateway.Common.Abstractions;

public interface IClientApplicationInfo
{
    string ApplicationName { get; }
    Version ApplicationVersion { get; }
    string Environment { get; }
}
