namespace AtendeLogo.ClientGateway.Common.Contracts;

public interface IHttpClientResilienceOptions
{
    int MaxRetryAttempts { get; }
    SemaphoreSlim ConcurrentLock { get; }
    TimeSpan RetryDelay { get; }
}
