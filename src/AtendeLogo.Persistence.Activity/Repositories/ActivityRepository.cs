using System.Linq.Expressions;
using AtendeLogo.Application.Abstractions.Persistence.Activities;
using AtendeLogo.Common;
using AtendeLogo.Domain.Entities.Activities;
using AtendeLogo.Persistence.Activity.Documents;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace AtendeLogo.Persistence.Activity.Repositories;

public class ActivityRepository : IActivityRepository
{
    private const int ACTIVITY_QUERY_LIMIT = 100;
    private readonly IMongoCollection<ActivityDocument> _collection;
    private readonly ILogger _logger;
     
    public ActivityRepository(
        IMongoDatabase database,
        ILogger<ActivityRepository> logger)
    {
        Guard.NotNull(database);

        _collection = database.GetCollection<ActivityDocument>("activities");
        _logger = logger;
    }

    public async Task<IEnumerable<ActivityBase>> GetAllAsync()
    {
        return await QueryAsync(_ => true);
    }

    private async Task<IEnumerable<ActivityBase>> QueryAsync(
        Expression<Func<ActivityDocument, bool>> filter,
        FindOptions? options = null,
        int limit = ACTIVITY_QUERY_LIMIT)
    {
        try
        {
            var documents = await _collection
                .Find(filter, options)
                .SortByDescending(d => d.ActivityAt)
                .Limit(limit)
                .ToListAsync();

            return documents.Select(ActivityDocumentMapper.MapToDomain);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error querying activities");
            throw;
        }
    }

    public async Task<ActivityBase?> GetByIdAsync(string id)
    {
        var result = await QueryAsync(d => d.Id == id, limit: 1);
        return result.FirstOrDefault();
    }

    public async Task AddAsync(ActivityBase activity)
    {
        var document = ActivityDocumentMapper.MapToDocument(activity);
        try
        {
            await _collection.InsertOneAsync(document);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error adding activity");
            throw;
        }
    }

    public async Task DeleteAsync(string id)
    {
        try
        {
            await _collection.DeleteOneAsync(d => d.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting activity");
            throw;
        }
    }
}
