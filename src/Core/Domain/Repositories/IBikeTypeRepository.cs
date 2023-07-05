namespace Domain.Repositories;

using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IBikeTypeRepository
{
	void Add(BikeType bikeType);

	Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default);

	Task<BikeType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

	Task<IEnumerable<BikeType>> GetAllAsync(CancellationToken cancellationToken = default);

	void Update(BikeType bikeType);

	Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}