﻿namespace Presentation.Controllers;

using Application.Entities.Bikes.Models;
using Application.Entities.Bikes.Queries.GetBikes;
using Application.Entities.BikeTypes.Commands.CreateBikeType;
using Application.Entities.BikeTypes.Models;
using Application.Entities.BikeTypes.Queries.GetAllBikeTypes;
using Domain.Entities;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BikeTypesController : ApiController
{
	public BikeTypesController(ISender sender) 
		: base(sender)
	{
	}

	[HttpGet]
	[ProducesResponseType(typeof(IEnumerable<SimpleBikeTypeResponse>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetBikeTypes(CancellationToken cancellationToken)
	{
		var result = await this.Sender.Send(new GetAllBikeTypesQuery(), cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
	}

	[HttpPost]
	[ProducesResponseType(typeof(SimpleBikeTypeResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IResult> CreateBikeType(
		CreateBikeTypeRequest request, 
		IValidator<CreateBikeTypeCommand> validator,
		CancellationToken cancellationToken)
	{
		var command = request.Adapt<CreateBikeTypeCommand>();
		var validation = validator.Validate(command);
		if (!validation.IsValid)
		{
			return Results.ValidationProblem(validation.ToDictionary());
		}

		var result = await this.Sender.Send(command, cancellationToken);
		return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
	}
}
