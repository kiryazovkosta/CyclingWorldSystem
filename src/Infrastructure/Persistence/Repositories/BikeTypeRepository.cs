namespace Persistence.Repositories;

using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class BikeTypeRepository : IBikeTypeRepository
{
	private readonly ApplicationDbContext _context;

	public BikeTypeRepository(ApplicationDbContext context)
	{
		this._context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public void Add(BikeType bikeType)
	{
		this._context
			.Set<BikeType>()
			.Add(bikeType);	
	}

	public async Task<BikeType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		return await this._context
			.Set<BikeType>()
			.SingleOrDefaultAsync(bt => bt.Id == id, cancellationToken);
	}


	public async Task<IEnumerable<BikeType>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		return await this._context
			.Set<BikeType>()
			.AsNoTracking()
			.ToListAsync(cancellationToken);
	}

	public async Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default)
	{
		return await this._context
			.Set<BikeType>()
			.AnyAsync(pt => pt.Name == name, cancellationToken);
	}

	public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var bikeType = await GetByIdAsync(id, cancellationToken);
        if (bikeType is not null)
        {

			this._context.Set<BikeType>().Remove(bikeType);
			return true;
		}

		return false;
	}

	public void Update(BikeType bikeType)
	{
		this._context
			.Set<BikeType>()
			.Update(bikeType);
	}
}