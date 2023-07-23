namespace Persistence.Repositories;

using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

public class WaypointRepository : IWaypointRepository
{
    private readonly ApplicationDbContext _context;

    public WaypointRepository(ApplicationDbContext context)
    {
        this._context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void AddRange(IEnumerable<Waypoint> waypoints)
    {
        this._context
            .Set<Waypoint>()
            .AddRange(waypoints);
    }

    public async Task UpdateActivityAsync(
        Guid gpxId, 
        Guid activityId, 
        CancellationToken cancellationToken)
    {
        await this._context.Set<Waypoint>()
            .Where(w => w.GpxId == gpxId)
            .ExecuteUpdateAsync(s => s.SetProperty(
                w => w.ActivityId,
                w => activityId), cancellationToken);
    }
}