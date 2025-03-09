namespace AtendeLogo.Shared.Contracts;

public interface IEndpointService
{
    public ServiceRole ServiceRole { get; }
    public string ServiceName { get; }
    public bool IsAllowAnonymous { get; }
}
