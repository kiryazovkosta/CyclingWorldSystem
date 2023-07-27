namespace Persistence.Repositories;

using Domain.Entities;
using Domain.Entities.Dtos;
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

    public async Task<IEnumerable<WaypointCoordinateDto>> GetCoordinatesAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        return await this._context
            .Set<Waypoint>()
            .Where(w => w.ActivityId == id)
            .OrderBy(w => w.OrderIndex)
            .Select(w => new WaypointCoordinateDto()
            {
                Latitude = w.Latitude,
                Longitude = w.Longitude
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<CenterCoordinateDto> GetCenterCoordinateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await this._context
            .Set<Waypoint>()
            .Where(w => w.ActivityId == id)
            .Select(w => new CenterCoordinateDto()
            {
                Longitude = w.Longitude,
                Latitude = w.Latitude,
            })
            .FirstAsync(cancellationToken);
    }
}