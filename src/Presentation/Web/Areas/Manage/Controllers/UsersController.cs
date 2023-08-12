namespace Web.Areas.Manage.Controllers;

using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Web.Controllers;
using Web.Models.Activities;

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
    public async Task<IActionResult> All(int? pageSize, int? pageNumber, string? orderBy)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }
        
        var inputModel = new QueryParameterInputModel()
        {
            PageSize = pageSize ?? 10,
            PageNumber = pageNumber ?? 1,
            OrderBy = orderBy
        };

        var parameters = new Dictionary<string, string>()
        {
            { "PageSize", inputModel.PageSize.ToString() },
            { "PageNumber", inputModel.PageNumber.ToString() },
            { "OrderBy", inputModel.OrderBy ?? string.Empty }
        };

        var usersResponse = await this.GetAsync<PagedUsersDataViewModel>("api/Users", token, parameters);
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
    
    [HttpGet]
    public async Task<IActionResult> AssignToRole(string id)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        Guid userId = Guid.Parse(id);

        var userRolesResponse = await this.GetAsync<IEnumerable<string>>(
            "api/Users/GetRoles/" + userId.ToString(), token);
        IEnumerable<string> userRoles = userRolesResponse.Value!.ToList();
        
        var allRolesResponse = await this.GetAsync<IEnumerable<RoleFullViewModel>>(
            "api/Roles", token);
        IEnumerable<string> allRoles = allRolesResponse.Value!.Select(r => r.Name).ToList();
        
        var model = new UserAssignedRoles()
        {
            UserId = userId,
            UserName = this.User?.Identity?.Name ?? string.Empty,
            MemberOfRoles = userRoles.ToList().ToList(),
            NotMemberOfRoles = allRoles.Except(userRoles, StringComparer.OrdinalIgnoreCase).ToList()
        };
        
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AssignToRole(UserWithRolesInputModel model)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        var result = 
            await this.PostAsync<UserWithRolesInputModel, Guid>("api/Users/AssignRoles", model, token);

        return RedirectToAction("All", "Users");
    }
}