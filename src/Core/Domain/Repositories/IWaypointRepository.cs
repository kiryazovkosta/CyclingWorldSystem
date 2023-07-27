// ------------------------------------------------------------------------------------------------
//  <copyright file="IWaypointRepository.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Domain.Repositories;

using Entities;
using Entities.Dtos;

public interface IWaypointRepository
{
    void AddRange(IEnumerable<Waypoint> waypoints);
    Task UpdateActivityAsync(Guid gpxId, Guid activityId, CancellationToken cancellationToken = default);

    Task<IEnumerable<WaypointCoordinateDto>> GetCoordinatesAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<CenterCoordinateDto> GetCenterCoordinateAsync(Guid id, CancellationToken cancellationToken = default);
}