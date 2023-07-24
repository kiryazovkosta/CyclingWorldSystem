namespace Web.Areas.Manage.Controllers;

using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Web.Controllers;

[Authorize(Roles = "Administrator")]
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
    public async Task<IActionResult> All()
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        var usersResponse = await this.GetAsync<IEnumerable<UserViewModel>>("api/Users", token);
        if (usersResponse.IsFailure)
        {
            return RedirectToAction("Index", "Management");
        }
        
        return View(usersResponse.Value);
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        var userModel = await this.GetAsync<UserInputModel>($"/api/Users/{id}", token);
        if (userModel.IsFailure)
        {
            return View();
        }

        var user = userModel.Value;
        if (user is null) 
        {
            return View();
        }
        
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UserInputModel userModel)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        if (!ModelState.IsValid)
        { 
            return View(userModel);
        }
        
        var result = await this.PutAsync<UserInputModel>("/api/Users", userModel, token);
        return RedirectToAction("All", "Users");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }
        
        var currentUserId = this.CurrentUserId();
        if (id == currentUserId)
        {
            return RedirectToAction("All", "Users");
        }

        await this.DeleteAsync("/api/Users/", Guid.Parse(id), token);
        return RedirectToAction("All", "Users");
    }
}