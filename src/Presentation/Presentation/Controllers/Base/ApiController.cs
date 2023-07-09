namespace Presentation.Controllers.Base;

using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
	protected readonly ISender Sender;

	protected ApiController(ISender sender)
	{
		this.Sender = sender;
	}
}