namespace Presentation.Controllers;

using Application.Identity.Users.Commands.CreateUser;
using Application.Identity.Users.Commands.LogInUser;
using Application.Identity.Users.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Base;
using Application.Identity.Users.Commands.ConfirmEmail;
using Application.Identity.Users.Commands.ResendConfirmEmail;

public sealed class AccountsController : ApiController
{
	public AccountsController(ISender sender) 
		: base(sender)
	{
	}

	[HttpPost("Register")]
	[ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CreateUser(
		[FromForm] CreateUserCommand request, CancellationToken cancellationToken)
	{
		var user = await this.Sender.Send(request, cancellationToken);
		return user.IsSuccess ? Ok(user.Value) : BadRequest(user.Error);
	}

	[HttpPost("LogIn")]
	[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> LogInUser(
	[FromBody] LoginUserRequest request, CancellationToken cancellationToken)
	{
		var command = request.Adapt<LogInUserCommand>();
		var token = await this.Sender.Send(command, cancellationToken);
		return token.IsSuccess ? Ok(token.Value) : BadRequest(token.Error);
	}

	[HttpPost("ConfirmEmail")]
	[ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> ConfirmEmail(
		[FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken)
	{
		var command = request.Adapt<ConfirmEmailCommand>();
		var token = await this.Sender.Send(command, cancellationToken);
		return token.IsSuccess ? Ok(token.Value) : BadRequest(token.Error);
	}

	[HttpPost("ResendConfirmEmail")]
	[ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> ConfirmEmail(
		[FromBody] ResendConfirmEmailRequest request, CancellationToken cancellationToken)
	{
		var command = request.Adapt<ResendConfirmEmailCommand>();
		var token = await this.Sender.Send(command, cancellationToken);
		return token.IsSuccess ? Ok(token.Value) : BadRequest(token.Error);
	}
}
