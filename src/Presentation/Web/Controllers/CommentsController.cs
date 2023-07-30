// ------------------------------------------------------------------------------------------------
//  <copyright file="CommentsController.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Controllers;

using Microsoft.AspNetCore.Mvc;
using Models.Comments;

public class CommentsController : AuthorizationController
{
    public CommentsController(
        IHttpContextAccessor httpContextAccessor, 
        IHttpClientFactory httpClientFactory, 
        IConfiguration configuration) 
        : base(httpContextAccessor, httpClientFactory, configuration)
    {
    }

    [HttpGet]
    public IActionResult Create(string id)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        var createComment = new CommentInputModel()
        {
            ActivityId = Guid.Parse(id)
        };

        return View(createComment);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommentInputModel model)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        var createResponse = await this.PostAsync<CommentInputModel, Guid>("/api/Comments", model, token);
        if (createResponse.IsFailure)
        {
            return View(model);
        }
        return RedirectToAction("Get", "Activities", new { id = model.ActivityId} );
    }
}