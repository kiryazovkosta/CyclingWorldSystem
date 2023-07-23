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
using Base;
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