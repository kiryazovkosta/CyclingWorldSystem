// ------------------------------------------------------------------------------------------------
//  <copyright file="GetWaypointsByActivityQueryHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Waypoints.Queries.GetWaypointsByActivity;

using Abstractions.Messaging;
using Domain.Repositories;
using Domain.Shared;
using Mapster;
using Models;

public class GetWaypointsByActivityQueryHandler
    : IQueryHandler<GetWaypointsByActivityQuery, IEnumerable<CoordinateResponse>>
{
    private readonly IWaypointRepository _waypoints;

    public GetWaypointsByActivityQueryHandler(IWaypointRepository waypointRepository)
    {
        this._waypoints = waypointRepository ?? throw new ArgumentNullException(nameof(waypointRepository));
    }

    public async Task<Result<IEnumerable<CoordinateResponse>>> Handle(
        GetWaypointsByActivityQuery request, 
        CancellationToken cancellationToken)
    {
        var coordinates =
            await this._waypoints.GetCoordinatesAsync(request.Id, cancellationToken);
        var response = coordinates.Adapt<IEnumerable<CoordinateResponse>>();
        return Result.Success(response);
    }
}