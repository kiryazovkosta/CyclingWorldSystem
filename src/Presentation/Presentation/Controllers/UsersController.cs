﻿namespace Presentation.Controllers;

using Application.Identity.Users.Commands.DeleteUser;
using Application.Identity.Users.Commands.UpdateUser;
using Application.Identity.Users.Commands.UpdateUserRoles;
using Application.Identity.Users.Models;
using Application.Identity.Users.Queries.GetAllUsers;
using Application.Identity.Users.Queries.GetUserById;
using Application.Identity.Users.Queries.GetUserRoles;
using Base;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class UsersController : ApiController
{
    public UsersController(ISender sender) 
        : base(sender)
    {
    }
    
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> GetUserById(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var result = await this.Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }
    
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllUsers(
        CancellationToken cancellationToken)
    {
        var query = new GetAllUsersQuery();
        var result = await this.Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
    [HttpGet("GetRoles/{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> GetUserRoles(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var query = new GetUserRolesCommand(id);
        var result = await this.Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }
    
    [HttpPut]
    [Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser(
        [FromBody] UpdateUserRequest request, 
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<UpdateUserCommand>();
        var result = await this.Sender.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Roles="Administrator")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> DeleteUser(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(id);
        var result = await this.Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
    }
    
    [HttpPost("AssignRoles")]
    [Authorize(Roles="Administrator")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignRolesToUser(
        [FromBody] UpdateUserRolesRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<UpdateUserRolesCommand>();
        var result = await this.Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}