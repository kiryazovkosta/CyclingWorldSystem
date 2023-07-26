namespace Domain.Repositories;

using Entities;

public interface IActivityRepository
{
    void Add(Activity activity);

    Task<IEnumerable<Activity>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task<Activity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}