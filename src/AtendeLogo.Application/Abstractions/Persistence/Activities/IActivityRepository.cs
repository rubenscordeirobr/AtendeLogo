using AtendeLogo.Domain.Entities.Activities;

namespace AtendeLogo.Application.Abstractions.Persistence.Activities;
public interface IActivityRepository
{
    Task<IEnumerable<ActivityBase>> GetAllAsync();
    Task<ActivityBase?> GetByIdAsync(string id);
    Task AddAsync(ActivityBase activity);
    Task DeleteAsync(string id);
}
