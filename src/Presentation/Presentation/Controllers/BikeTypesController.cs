namespace Presentation.Controllers;

using Application.Entities.Bikes.Models;
using Application.Entities.Bikes.Queries.GetBikes;
using Application.Entities.BikeTypes.Models;
using Application.Entities.BikeTypes.Queries.GetAllBikeTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BikeTypesController : ApiController
{
	public BikeTypesController(ISender sender) 
		: base(sender)
	{
	}

	[HttpGet]
	[Authorize]
	[ProducesResponseType(typeof(IEnumerable<SimpleBikeTypeResponse>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetBikes(CancellationToken cancellationToken)
	{
		var result = await this.Sender.Send(new GetAllBikeTypesQuery(), cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
	}
}
