namespace AtendeLogo.Shared.Abstractions;

public interface IApplicationInfo
{
    string ApplicationName { get; }
    Version ApplicationVersion { get; }
    string Environment { get; }
}
