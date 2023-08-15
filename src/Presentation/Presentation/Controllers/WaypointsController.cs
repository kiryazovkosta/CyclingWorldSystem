// ------------------------------------------------------------------------------------------------
//  <copyright file="WaypointsController.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Presentation.Controllers;

using Application.Entities.Waypoints.Models;
using Application.Entities.Waypoints.Queries.GetWaypointsByActivity;
using Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class WaypointsController: ApiController
{
    public WaypointsController(ISender sender) 
        : base(sender)
    {
    }
    
    [HttpGet("Coordinates/{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(CoordinateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCoordinates(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetWaypointsByActivityQuery(id);
        var result = await this.Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}