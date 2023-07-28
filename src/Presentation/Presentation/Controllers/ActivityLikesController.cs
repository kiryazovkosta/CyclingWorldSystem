// ------------------------------------------------------------------------------------------------
//  <copyright file="ActivityLikesController.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Presentation.Controllers;

using Application.Entities.Likes.Commands.CreateActivityLike;
using Application.Entities.Likes.Models;
using Base;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class ActivityLikesController : ApiController
{
    public ActivityLikesController(ISender sender) 
        : base(sender)
    {
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Like(
        [FromBody] CreateActivityLikeRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<CreateActivityLikeCommand>();
        var result = await this.Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }
}