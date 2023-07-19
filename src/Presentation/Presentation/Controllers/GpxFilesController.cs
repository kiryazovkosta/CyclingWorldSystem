

namespace Presentation.Controllers;

using Application.Entities.Bikes.Models;
using Application.Entities.GpxFiles.Commands.ParseGpxFile;
using Application.Entities.GpxFiles.Models;
using Application.Services;
using Common.Constants;
using Infrastructure.Extensions;
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

public class GpxFilesController : ApiController
{
	public GpxFilesController(ISender sender) : base(sender)
	{
	}

	[HttpPost]
	[Authorize]
	[ProducesResponseType(typeof(GpxFileResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> ParseGpxFile(
		[FromForm] GpxFileRequest request,
		CancellationToken cancellationToken)
	{
		if (!request.Files.Any())
		{
			return BadRequest(GlobalMessages.GpxFile.CollectionIsEmpty);
		}

		var file = request.Files.First();
		var xml = await file.ReadAsStringAsync();
		var command = new ParseGpxFileCommand(xml);
		var result = await this.Sender.Send(command, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
	}
}
