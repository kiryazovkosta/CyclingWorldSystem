// ------------------------------------------------------------------------------------------------
//  <copyright file="ActivityRepository.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Persistence.Repositories;

using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

public class ActivityRepository : IActivityRepository
{
    private readonly ApplicationDbContext _context;

    public ActivityRepository(ApplicationDbContext context)
    {
        this._context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Add(Activity activity)
    {
        this._context.Set<Activity>().Add(activity);
    }

    public async Task<IEnumerable<Activity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await this._context
            .Set<Activity>()
            .ToListAsync(cancellationToken);
    }

    public async Task<Activity> GetByIdAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}