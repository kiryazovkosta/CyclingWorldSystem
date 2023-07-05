namespace Presentation.Controllers;

using Application.Entities.Bikes.Commands.CreateBike;
using Application.Entities.Bikes.Commands.DeleteBike;
using Application.Entities.Bikes.Commands.UpdateBike;
using Application.Entities.Bikes.Models;
using Application.Entities.Bikes.Queries.GetBikeById;
using Application.Entities.Bikes.Queries.GetBikes;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

public sealed class BikesController : ApiController
{
    public BikesController(ISender sender)
		: base(sender)
    {  
    }

	[HttpGet("{id:guid}")]
	[ProducesResponseType(typeof(BikeResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IResult> GetBikeById(
		Guid id, 
		CancellationToken cancellationToken)
	{
		var query = new GetBikeByIdQuery(id);
		var result = await this.Sender.Send(query, cancellationToken);
		return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
	}

	[HttpGet]
	[Authorize]
	[ProducesResponseType(typeof(BikeResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetBikes(
		CancellationToken cancellationToken)
	{
		var query = new GetBikesQuery();
		var result = await this.Sender.Send(query, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
	}

	[HttpPost]
	[Authorize]
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IResult> CreateBike(
		[FromBody] CreateBikeRequest request, 
		IValidator<CreateBikeCommand> validator,
		CancellationToken cancellationToken)
	{
		var command = request.Adapt<CreateBikeCommand>();
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
	public async Task<IActionResult> UpdateBike(
		[FromBody] UpdateBikeRequest request, 
		CancellationToken cancellationToken)
	{
		var command = request.Adapt<UpdateBikeCommand>();
		var result = await this.Sender.Send(command, cancellationToken);
		return result.IsSuccess ? NoContent() : BadRequest(result.Error);
	}

	[HttpDelete("{id:guid}")]
	[ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IResult> DeleteBike(
		Guid id,
		CancellationToken cancellationToken)
	{
		var command = new DeleteBikeCommand(id);
		var result = await this.Sender.Send(command, cancellationToken);
		return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
	}
}