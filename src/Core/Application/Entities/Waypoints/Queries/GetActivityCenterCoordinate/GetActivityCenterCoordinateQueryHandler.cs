// ------------------------------------------------------------------------------------------------
//  <copyright file="GetActivityCenterCoordinateQueryHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Waypoints.Queries.GetActivityCenterCoordinate;

using Abstractions.Messaging;
using Domain.Repositories;
using Domain.Shared;
using Mapster;
using Models;

public class GetActivityCenterCoordinateQueryHandler
    : IQueryHandler<GetActivityCenterCoordinateQuery, CenterCoordinateResponse>
{
    private readonly IWaypointRepository _waypoints;

    public GetActivityCenterCoordinateQueryHandler(IWaypointRepository waypoints)
    {
        this._waypoints = waypoints ?? throw new ArgumentNullException(nameof(waypoints));
    }

    public async Task<Result<CenterCoordinateResponse>> Handle(
        GetActivityCenterCoordinateQuery request, 
        CancellationToken cancellationToken)
    {
        var coordinate = await this._waypoints.GetCenterCoordinateAsync(request.ActivityId, cancellationToken);
        var response = coordinate.Adapt<CenterCoordinateResponse>();
        return Result.Success(response);
    }
}