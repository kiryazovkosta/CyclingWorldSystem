using Domain.Entities;
using Domain.Entities.Dtos;

namespace Domain.Repositories;

public interface IBikeRepository
{
	void Add(Bike bike);

	Task<Bike?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

	Task<List<BikeResponseDto>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken = default);

	void Update(Bike bike);

	Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}