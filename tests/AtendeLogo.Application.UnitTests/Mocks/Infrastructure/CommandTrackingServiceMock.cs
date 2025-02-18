using AtendeLogo.Common;

namespace AtendeLogo.Application.UnitTests.Mocks.Infrastructure;

internal class CommandTrackingServiceMock : ICommandTrackingService
{
    public Task<bool> ExistsAsync(Guid clientRequestId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }

    public Task<Result<T>?> TryGetResultAsync<T>(Guid clientRequestId, CancellationToken cancellationToken = default) where T : notnull
    {
        return Task.FromResult<Result<T>?>(null);
    }

    public Task TrackAsync<T>(Guid clientRequestId, Result<T> result) where T : notnull
    {
        return Task.CompletedTask;
    }
}
