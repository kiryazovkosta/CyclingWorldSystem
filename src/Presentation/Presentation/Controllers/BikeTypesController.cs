namespace Presentation.Controllers;
using Application.Entities.BikeTypes.Commands.CreateBikeType;
using Application.Entities.BikeTypes.Commands.DeleteBikeType;
using Application.Entities.BikeTypes.Commands.UpdateBikeType;
using Application.Entities.BikeTypes.Models;
using Application.Entities.BikeTypes.Queries.ExistsBikeTypeById;
using Application.Entities.BikeTypes.Queries.GetAllBikeTypes;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Entities.BikeTypes.Queries.GetBikeTypeById;

[Authorize]
public class BikeTypesController : ApiController
{
	public BikeTypesController(ISender sender) 
		: base(sender)
	{
	}
	
	[HttpGet("{id:guid}")]
	[Authorize]
	[ProducesResponseType(typeof(SimpleBikeTypeResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IResult> GetBikeTypeById(
		Guid id, 
		CancellationToken cancellationToken)
	{
		var query = new GetBikeTypeByIdQuery(id);
		var result = await this.Sender.Send(query, cancellationToken);
		return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
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
	public async Task<IResult> DeleteBikeType(
		Guid id, 
		CancellationToken cancellationToken)
	{
		var command = new DeleteBikeTypeCommand(id);
		var result = await this.Sender.Send(command, cancellationToken);
		return result.IsSuccess ? Results.NoContent() : Results.BadRequest(result.Error);
	}


    [HttpGet("Exists")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> ExistsById(
		[FromQuery] ExistsBikeTypeByIsQuery request,
		CancellationToken cancellationToken)
    {
        var command = request.Adapt<ExistsBikeTypeByIsQuery>();
        var result = await this.Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }

}
