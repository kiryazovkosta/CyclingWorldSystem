namespace Persistence.Repositories;

using Domain.Abstractions;
using Domain.Entities;
using System;

public sealed class BikeRepository : IBikeRepository
{
	private readonly ApplicationDbContext _dbContext;

	public BikeRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
	}

	public void Add(Bike bike)
	{
		this._dbContext.Set<Bike>().Add(bike);
	}
}