// ------------------------------------------------------------------------------------------------
//  <copyright file="ActivityLikeRepository.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Persistence.Repositories;

using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

public class ActivityLikeRepository : IActivityLikeRepository
{
    private readonly ApplicationDbContext _context;

    public ActivityLikeRepository(ApplicationDbContext context)
    {
        this._context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Add(ActivityLike activityLike)
    {
        this._context.Set<ActivityLike>().Add(activityLike);
    }

    public void Remove(ActivityLike activityLike)
    {
        this._context.Set<ActivityLike>().Remove(activityLike);
    }

    public async Task<ActivityLike?> GetByIdAsync(
        Guid activityId, 
        Guid userId, 
        CancellationToken cancellationToken)
    {
        return await this._context
            .Set<ActivityLike>()
            .FirstOrDefaultAsync(al => al.ActivityId == activityId 
                                    && al.UserId == userId, 
                                cancellationToken);
    }
}