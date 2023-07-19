namespace Persistence.Repositories;

using Domain.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Domain.Entities.Dtos;

public sealed class BikeRepository : IBikeRepository
{
	private readonly ApplicationDbContext _dbContext;

	public BikeRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
	}

	public void Add(Bike bike)
	{
		this._dbContext
			.Set<Bike>()
			.Add(bike);
	}

	public async Task<Bike?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		return await _dbContext
			.Set<Bike>()
			.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
	}

    public async Task<List<BikeResponseDto>> GetAllByUserAsync(
		Guid userId, 
		CancellationToken cancellationToken = default)
    {
        return await this._dbContext
			.Set<Bike>()
			.AsNoTracking()
			.Include(b => b.BikeType)
			.Where(b => b.UserId == userId)
			.Select(b => 
				new BikeResponseDto(b.Id, b.Name, b.BikeTypeId, b.BikeType.Name, b.Weight, b.Brand, b.Model, b.Notes, b.UserId))
			.ToListAsync(cancellationToken);
    }

	public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var bike = await this.GetByIdAsync(id, cancellationToken);
		if (bike is null)
		{
			return false;
		}
		
		this._dbContext
			.Set<Bike>()
			.Remove(bike);
		return true;

	}

	public void Update(Bike bike)
	{
		this._dbContext
			.Set<Bike>()
			.Update(bike);
	}
}