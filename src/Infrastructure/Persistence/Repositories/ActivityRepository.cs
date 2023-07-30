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
            .Include(a => a.User)
            .Include(a => a.Images)
            .ToListAsync(cancellationToken);
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
}