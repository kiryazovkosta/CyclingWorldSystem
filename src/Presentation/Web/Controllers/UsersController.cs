// ------------------------------------------------------------------------------------------------
//  <copyright file="UsersController.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Controllers;

using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Users;

[Authorize(Roles = "User,Manager")]
public class UsersController : AuthorizationController
{
    public UsersController(
        IHttpContextAccessor httpContextAccessor, 
        IHttpClientFactory httpClientFactory, 
        IConfiguration configuration) 
        : base(httpContextAccessor, httpClientFactory, configuration)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        var userId =  base.CurrentUserId();
        var userResponse = await this.GetAsync<UserProfileViewModel>($"/api/Users/{userId}", token);
        if (userResponse.IsFailure)
        {
            return View();
        }

        return View(userResponse.Value);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeDetails([FromForm] ChangeUserDetailsModel model)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        var result =
            await this.PostAsync<ChangeUserDetailsModel, bool>("api/Accounts/Update", model, token);

        var responseModel = new ResultMessageModel();
        if (result.IsFailure)
        {
            responseModel.Error = result?.Error?.Message ?? GlobalMessages.GlobalError;
        }
        else
        {
            responseModel.Success =  "User details are changed successfully.";
        }
        
        return RedirectToAction("Index", "Home", responseModel);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword([FromForm] ChangeUserPasswordModel model)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        var result =
            await this.PostAsync<ChangeUserPasswordModel, bool>("api/Accounts/ChangePassword", model, token);

        var responseModel = new ResultMessageModel();
        if (result.IsFailure)
        {
            responseModel.Error = result?.Error?.Message ?? GlobalMessages.GlobalError;
        }
        else
        {
            responseModel.Success =  "User password is changed successfully.";
        }
        
        return RedirectToAction("Index", "Home", responseModel);
    }
}