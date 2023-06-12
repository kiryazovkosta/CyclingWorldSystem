﻿namespace Presentation.Controllers;

using Application.Entities.Bikes.Commands.CreateBike;
using Application.Entities.Bikes.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;

public sealed class BikesController : ApiController
{
    public BikesController(ISender sender)
		: base(sender)
    {  
    }

    [HttpPost]
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CreateBike(
		[FromBody] CreateBikeRequest request, CancellationToken cancellationToken)
	{
		var command = request.Adapt<CreateBikeCommand>();
		var bikeId = await this.Sender.Send(command, cancellationToken);
		return Ok(bikeId);
	}
}