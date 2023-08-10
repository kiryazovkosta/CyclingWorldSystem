namespace Domain.Repositories;

using Entities;
using Primitives;

public interface IActivityRepository
{
    void Add(Activity activity);

    Task<IPagedList<Activity, Guid>>  GetAllAsync(QueryParameter parameters, CancellationToken cancellationToken = default);
    
    Task<Activity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}