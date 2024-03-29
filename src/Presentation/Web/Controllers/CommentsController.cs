﻿// ------------------------------------------------------------------------------------------------
//  <copyright file="CommentsController.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Controllers;

using AspNetCoreHero.ToastNotification.Abstractions;
using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Comments;

[Authorize(Roles = "User,Manager")]
public class CommentsController : AuthorizationController
{
    private readonly INotyfService _notification;
    
    public CommentsController(
        IHttpContextAccessor httpContextAccessor, 
        IHttpClientFactory httpClientFactory, 
        IConfiguration configuration,
        INotyfService notification) 
        : base(httpContextAccessor, httpClientFactory, configuration)
    {
        ArgumentNullException.ThrowIfNull(notification);
        this._notification = notification;
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
            var errorMessage = createResponse?.Error?.Message ?? GlobalMessages.GlobalError;
            this._notification.Error(errorMessage);
            return View(model);
        }
        return RedirectToAction("Get", "Activities", new { id = model.ActivityId} );
    }

    [HttpGet]
    public async Task<IActionResult> Update(string id)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        var commentResult = await this.GetAsync<CommentInputModel>($"/api/Comments/{id}", token);
        if (commentResult.IsFailure)
        {
            var errorMessage = commentResult?.Error?.Message ?? GlobalMessages.GlobalError;
            this._notification.Error(errorMessage);
            return View();
        }

        var commentModel = commentResult.Value;
        if (commentModel is null) 
        {
            return View();
        }
        
        return View(commentModel);
    }

    [HttpPost]
    public async Task<IActionResult> Update(CommentInputModel model)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        var createResponse = await this.PutAsync("/api/Comments", model, token);
        if (createResponse.IsFailure)
        {
            var errorMessage = createResponse?.Error?.Message ?? GlobalMessages.GlobalError;
            this._notification.Error(errorMessage);
            return View(model);
        }

        return RedirectToAction("Get", "Activities", new { id = model.ActivityId} );
    }

    [HttpGet]
    public async Task<IActionResult> Remove(string id)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        var commentResult = await this.GetAsync<CommentInputModel>($"/api/Comments/{id}", token);
        var commentModel = commentResult.Value;

        await this.DeleteAsync("api/Comments/", Guid.Parse(id), token);

        return RedirectToAction("Get", "Activities", new { id = commentModel?.ActivityId} );
    }
}