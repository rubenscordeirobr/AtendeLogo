using AtendeLogo.Domain.Entities.Activities;

namespace AtendeLogo.TestCommon.Mocks.Repositories;

public class ActivityRepositoryMock : IActivityRepository
{
    public Task<IEnumerable<ActivityBase>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<ActivityBase>>([]);
    }

    public Task<ActivityBase?> GetByIdAsync(string id)
    {
        return Task.FromResult<ActivityBase?>(null);
    }

    public Task AddAsync(ActivityBase activity)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string id)
    {
        return Task.CompletedTask;
    }
}
