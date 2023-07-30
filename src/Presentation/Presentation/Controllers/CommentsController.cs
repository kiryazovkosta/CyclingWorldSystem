// ------------------------------------------------------------------------------------------------
//  <copyright file="CommentsController.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Presentation.Controllers;

using Application.Entities.Bikes.Commands.CreateBike;
using Application.Entities.Bikes.Models;
using Application.Entities.Comments.Commands.CreateComment;
using Application.Entities.Comments.Models;
using Base;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class CommentsController: ApiController
{
    public CommentsController(ISender sender) 
        : base(sender)
    {
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateComment(
        [FromBody] CreateCommentRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<CreateCommentCommand>();
        var result = await this.Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }
}