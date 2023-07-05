using Domain.Entities;

namespace Domain.Repositories;

public interface IBikeRepository
{
	void Add(Bike bike);

	Task<Bike?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

	Task<IEnumerable<Bike>> GetAllAsync(CancellationToken cancellationToken = default);

	void Update(Bike bike);

	void Delete(Bike bike);
}