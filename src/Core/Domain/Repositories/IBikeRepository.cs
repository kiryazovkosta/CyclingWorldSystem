namespace Domain.Abstractions;

using Domain.Entities;

public interface IBikeRepository
{
	void Add(Bike bike);
}
