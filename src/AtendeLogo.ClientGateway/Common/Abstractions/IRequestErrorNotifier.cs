
namespace AtendeLogo.ClientGateway.Common.Abstractions;

public interface IRequestErrorNotifier
{
    Task NotifyRequestErrorAsync(Error error, Uri requestUri);
}
