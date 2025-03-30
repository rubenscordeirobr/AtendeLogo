namespace AtendeLogo.Shared.Contracts;

public interface IEndpointService
{
    public string ServiceName { get; }
    public ServiceRole ServiceRole { get; }
}
