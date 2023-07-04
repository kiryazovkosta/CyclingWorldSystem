namespace Domain.Repositories;

using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IBikeTypeRepository
{
	void Add(BikeType bikeType);

	Task<IEnumerable<BikeType>> GetAllAsync(CancellationToken cancellationToken = default);
}