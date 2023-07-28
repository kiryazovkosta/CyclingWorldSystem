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
}