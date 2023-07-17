namespace Presentation.Controllers;

using Application.Entities.Bikes.Models;
using Application.Entities.Images.Commands.CreateImage;
using Application.Entities.Images.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;

public class ImagesController : ApiController
{
    public ImagesController(ISender sender) 
        : base(sender)
    {
    }

    [HttpPost]
    //[Authorize]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadImage(
        [FromForm] CreateImageCommand command,
        CancellationToken cancellationToken)
    {
        var result = await this.Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
    }
}
