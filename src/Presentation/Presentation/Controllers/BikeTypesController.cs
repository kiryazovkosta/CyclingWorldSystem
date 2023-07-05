namespace Presentation.Controllers;
using Application.Entities.BikeTypes.Commands.CreateBikeType;
using Application.Entities.BikeTypes.Commands.DeleteBikeType;
using Application.Entities.BikeTypes.Commands.UpdateBikeType;
using Application.Entities.BikeTypes.Models;
using Application.Entities.BikeTypes.Queries.GetAllBikeTypes;
using FluentValidation;
using Mapster;
using MediatR;
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

	[HttpPut]
	[ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IResult> UpdateBikeType(
		UpdateBikeTypeRequest request,
		IValidator<UpdateBikeTypeCommand> validator,
		CancellationToken cancellationToken)
	{
		var command = request.Adapt<UpdateBikeTypeCommand>();
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
	public async Task<IResult> DeleteProductType(
		Guid id, 
		CancellationToken cancellationToken)
	{
		var command = new DeleteBikeTypeCommand(id);
		var result = await this.Sender.Send(command, cancellationToken);
		return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
	}
}
