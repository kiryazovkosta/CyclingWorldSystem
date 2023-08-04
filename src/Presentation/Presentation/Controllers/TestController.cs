// ------------------------------------------------------------------------------------------------
//  <copyright file="TestController.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Presentation.Controllers;

using Application.Entities.Emails.SendEmail;
using Base;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class TestController : ApiController
{
    public TestController(ISender sender) : base(sender)
    {
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail(
        [FromBody] SendEmailCommand command,
        CancellationToken cancellationToken)
    {
        var result = await this.Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok("success") : BadRequest(result.Error);
    }
}