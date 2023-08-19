// ------------------------------------------------------------------------------------------------
//  <copyright file="ActivitiesController.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Presentation.Controllers;

using Application.Entities.Activities.Commands.CreateActivity;
using Application.Entities.Activities.Models;
using Application.Entities.Activities.Queries.GetActivityById;
using Application.Entities.Activities.Queries.GetAll;
using Application.Entities.Activities.Queries.GetMine;
using Base;
using Domain;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class ActivitiesController: ApiController
{
    public ActivitiesController(ISender sender) : base(sender)
    {
    }
    
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ActivityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> GetActivityById(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var query = new GetActivityByIdQuery(id);
        var result = await this.Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }
    
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(SimplyActivityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllActivities(
        [FromQuery] QueryParameter parameters,
        CancellationToken cancellationToken)
    {
        var result = await this.Sender.Send(new GetAllActivitiesQuery(parameters), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
    [HttpGet("GetMine/{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(List<MyActivityResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> GetMineActivity(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var query = new GetMineQuery(id);
        var result = await this.Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }
    
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateActivity(
        [FromBody] CreateActivityRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<CreateActivityCommand>();
        var result = await this.Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }
}