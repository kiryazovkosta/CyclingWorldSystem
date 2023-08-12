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

        SetSortParam(orderBy);
        var parameters = new Dictionary<string, string>()
        {
            { "PageSize", (pageSize ?? 10).ToString() },
            { "PageNumber", (pageNumber ?? 1).ToString() },
            { "OrderBy", orderBy ?? string.Empty }
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
    
    private void SetSortParam(string? orderBy)
    {
        ViewData["SoftParamUserName"] = "UserName";
        ViewData["SoftParamEmail"] = "Email";
        ViewData["SoftParamFirstName"] = "FirstName";
        ViewData["SoftParamLastName"] = "LastName";
        ViewData["SoftParamIsConfirmed"] = "IsConfirmed";

        ViewData["SoftIconUserName"] = "";
        ViewData["SoftIconEmail"] = "";
        ViewData["SoftIconFirstName"] = "";
        ViewData["SoftIconLastName"] = "";
        ViewData["SoftIconIsConfirmed"] = "";

        switch (orderBy ?? string.Empty)
        {
            case "UserName desc":
                ViewData["SoftParamUserName"] = "UserName";
                ViewData["SoftIconUserName"] = "fa-solid fa-arrow-up-wide-short";
                break;
            case "Email":
                ViewData["SoftParamEmail"] = "Email desc";
                ViewData["SoftIconEmail"] = "fa-solid fa-arrow-up-short-wide";
                break;
            case "Email desc":
                ViewData["SoftParamEmail"] = "Email";
                ViewData["SoftIconEmail"] = "fa-solid fa-arrow-up-wide-short";
                break;
            case "FirstName":
                ViewData["SoftParamFirstName"] = "FirstName desc";
                ViewData["SoftIconFirstName"] = "fa-solid fa-arrow-up-short-wide";
                break;
            case "FirstName desc":
                ViewData["SoftParamFirstName"] = "FirstName";
                ViewData["SoftIconFirstName"] = "fa-solid fa-arrow-up-wide-short";
                break;
            case "LastName":
                ViewData["SoftParamLastName"] = "LastName desc";
                ViewData["SoftIconLastName"] = "fa-solid fa-arrow-up-short-wide";
                break;
            case "LastName desc":
                ViewData["SoftParamLastName"] = "LastName";
                ViewData["SoftIconLastName"] = "fa-solid fa-arrow-up-wide-short";
                break;
            case "IsConfirmed":
                ViewData["SoftParamIsConfirmed"] = "IsConfirmed desc";
                ViewData["SoftIconIsConfirmed"] = "fa-solid fa-arrow-up-short-wide";
                break;
            case "IsConfirmed desc":
                ViewData["SoftParamIsConfirmed"] = "IsConfirmed";
                ViewData["SoftIconIsConfirmed"] = "fa-solid fa-arrow-up-wide-short";
                break;
            default:
                ViewData["SoftParamUserName"] = "UserName desc";
                ViewData["SoftIconUserName"] = "fa-solid fa-arrow-up-short-wide";
                break;
        }
    }
}