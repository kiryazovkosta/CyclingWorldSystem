namespace Persistence.Repositories;

using Domain;
using Domain.Entities;
using Domain.Primitives;
using Domain.Repositories;
using Infrastructure;
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

    public async Task<IPagedList<Activity, Guid>> GetAllAsync(
        QueryParameter parameters,
        CancellationToken cancellationToken = default)
    {
        return await this._context
            .Set<Activity>()
            .Include(a => a.User)
            .Include(a => a.Images)
            .AsNoTracking()
            .Sort("CreatedOn desc", null)
            .Where(a => string.IsNullOrEmpty(parameters.SearchBy) || 
                (a.Description.Contains(parameters.SearchBy) || a.Title.Contains(parameters.SearchBy)))
            .ToPagedListAsync<Activity, Guid>(
                parameters.PageNumber,
                parameters.PageSize,
                cancellationToken);
    }

    public async Task<Activity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await this._context
            .Set<Activity>()
            .Include(a => a.User)
            .Include(a => a.Bike)
            .Include(a => a.Images)
            .Include(a => a.Likes)
            .Include(a => a.Comments)
            .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await this._context.Set<Activity>()
            .AnyAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<List<Activity>> GetAllMineAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await this._context
            .Set<Activity>()
            .AsNoTracking()
            .Include(a => a.Bike)
            .Include(a => a.Likes)
            .Include(a => a.Comments)
            .Where(a => a.UserId == userId)
            .Sort("CreatedOn desc", null)
            .ToListAsync(cancellationToken);
    }
}