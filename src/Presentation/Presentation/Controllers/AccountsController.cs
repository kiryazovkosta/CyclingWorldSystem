namespace Presentation.Controllers;

using Application.Identity.Users.Commands.CreateUser;
using Application.Identity.Users.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;

public sealed class AccountsController : ApiController
{
	public AccountsController(ISender sender) 
		: base(sender)
	{
	}

	[HttpPost]
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CreateUser(
		[FromBody] CreateUserRequest request, CancellationToken cancellationToken)
	{
		var command = request.Adapt<CreateUserCommand>();
		var bikeId = await this.Sender.Send(command, cancellationToken);
		return Ok(bikeId);
	}
}
