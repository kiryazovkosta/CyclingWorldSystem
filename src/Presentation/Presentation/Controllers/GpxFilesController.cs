

namespace Presentation.Controllers;

using Application.Entities.GpxFiles.Commands.ParseGpxFile;
using Application.Entities.GpxFiles.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;
using System.Threading.Tasks;

public class GpxFilesController : ApiController
{
	public GpxFilesController(ISender sender) 
		: base(sender)
	{
	}

	[HttpPost]
	[Authorize]
	[ProducesResponseType(typeof(GpxFileResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> ParseGpxFile(
		GpxFileRequest request,
		CancellationToken cancellationToken)
	{
		var command = new ParseGpxFileCommand(request.GpxFile);
		var result = await this.Sender.Send(command, cancellationToken);
		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
	}
}
