﻿namespace Presentation.Controllers;

using Application.Identity.Roles.Commands.CreateRole;
using Application.Identity.Roles.Commands.DeleteRole;
using Application.Identity.Roles.Commands.UpdateRole;
using Application.Identity.Roles.Models;
using Application.Identity.Roles.Queries.GetAllRoles;
using Application.Identity.Roles.Queries.GetRoleById;
using Base;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Administrator")]
public class RolesController : ApiController
{
    public RolesController(ISender sender) 
        : base(sender)
    {
    }
    
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(RoleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> GetRoleById(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var query = new GetRoleByIdQuery(id);
        var result = await this.Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }
    
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(RoleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllRoles(
        CancellationToken cancellationToken)
    {
        var query = new GetAllRolesQuery();
        var result = await this.Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateRole(
        [FromBody] CreateRoleRequest request,
        IValidator<CreateRoleCommand> validator,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<CreateRoleCommand>();
        var validation = validator.Validate(command);
        if (!validation.IsValid)
        {
            return Results.ValidationProblem(validation.ToDictionary());
        }
        
        var result = await this.Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }


    [HttpPut]
    [Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> UpdateBike(
        [FromBody] UpdateRoleRequest request,
        IValidator<UpdateRoleCommand> validator,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<UpdateRoleCommand>();
        var validation = validator.Validate(command);
        if (!validation.IsValid)
        {
            return Results.ValidationProblem(validation.ToDictionary());
        }
        var result = await this.Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> DeleteRole(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteRoleCommand(id);
        var result = await this.Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
    }
}