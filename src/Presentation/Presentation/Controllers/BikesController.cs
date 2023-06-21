namespace Presentation.Controllers;

using Application.Entities.Bikes.Commands.CreateBike;
using Application.Entities.Bikes.Commands.DeleteBike;
using Application.Entities.Bikes.Commands.EditBike;
using Application.Entities.Bikes.Models;
using Application.Entities.Bikes.Queries.GetBikeById;
using Application.Entities.Bikes.Queries.GetBikes;
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

	[HttpGet("GetById")]
	[ProducesResponseType(typeof(BikeResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetBikeById(
		[FromQuery] GetBikeByIdRequest request, CancellationToken cancellationToken)
	{
		var query = request.Adapt<GetBikeByIdQuery>();
		var result = await this.Sender.Send(query, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
	}

	[HttpGet]
	[ProducesResponseType(typeof(BikeResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetBikes(CancellationToken cancellationToken)
	{
		var query = new GetBikesQuery();
		var result = await this.Sender.Send(query, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
	}

	[HttpPost]
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CreateBike(
		[FromBody] CreateBikeRequest request, CancellationToken cancellationToken)
	{
		var command = request.Adapt<CreateBikeCommand>();
		var result = await this.Sender.Send(command, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
	}

	[HttpPut]
	[ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> EditBike(
		[FromBody] EditBikeRequest request, CancellationToken cancellationToken)
	{
		var command = request.Adapt<EditBikeCommand>();
		var result = await this.Sender.Send(command, cancellationToken);
		return result.IsSuccess ? NoContent() : BadRequest(result.Error);
	}

	[HttpDelete]
	[ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> DeleteBike(
		[FromBody] DeleteBikeRequest request, CancellationToken cancellationToken)
	{
		var command = request.Adapt<DeleteBikeCommand>();
		var result = await this.Sender.Send(command, cancellationToken);
		return result.IsSuccess ? NoContent() : BadRequest(result.Error);
	}
}