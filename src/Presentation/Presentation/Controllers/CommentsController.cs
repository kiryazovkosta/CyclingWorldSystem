// ------------------------------------------------------------------------------------------------
//  <copyright file="CommentsController.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Presentation.Controllers;

using Application.Entities.Comments.Commands.CreateComment;
using Application.Entities.Comments.Commands.DeleteComment;
using Application.Entities.Comments.Commands.UpdateComment;
using Application.Entities.Comments.Models;
using Application.Entities.Comments.Queries.GetCommentById;
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

    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> GetCommentById(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var query = new GetCommentByIdQuery(id);
        var result = await this.Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateComment(
        [FromBody] CreateCommentRequest request,
        IValidator<CreateCommentCommand> validator,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<CreateCommentCommand>();
        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
        var result = await this.Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }

    [HttpPut]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> UpdateBike(
        [FromBody] UpdateCommentRequest request,
        IValidator<UpdateCommentCommand> validator,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<UpdateCommentCommand>();
        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
        var result = await this.Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> DeleteComment(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCommentCommand(id);
        var result = await this.Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
    }
}