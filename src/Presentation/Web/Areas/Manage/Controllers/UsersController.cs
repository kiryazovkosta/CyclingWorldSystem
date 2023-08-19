namespace Web.Areas.Manage.Controllers;

using System.Text.Encodings.Web;
using AspNetCoreHero.ToastNotification.Abstractions;
using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Web.Controllers;
using Web.Models;
using Web.Models.Activities;
using Web.Models.Authorization;

[Authorize(Roles = "Administrator")]
public class UsersController : AuthorizationController
{
    private readonly INotyfService _notification;
    
    public UsersController(
        IHttpContextAccessor httpContextAccessor, 
        IHttpClientFactory httpClientFactory, 
        IConfiguration configuration,
        INotyfService notification) 
        : base(httpContextAccessor, httpClientFactory, configuration)
    {
        this._notification = notification ?? throw new ArgumentNullException(nameof(notification));
    }

    [HttpGet]
    public async Task<IActionResult> All(int? pageSize, int? pageNumber, string? orderBy)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        var sortedCollection = SetSortParam(orderBy);
        ViewData["SortedCollection"] = sortedCollection;
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
    public IActionResult Create()
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }
        
        var model = new RegisterUserInputModel();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(RegisterUserInputModel model)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }
        
        var result = await this.PostMultipartAsync<RegisterUserInputModel, Guid>("api/Accounts/Register", model, token);
        if (result.IsFailure)
        {
            var message = result?.Error?.Message ?? GlobalMessages.GlobalError;
            var encoded = HtmlEncoder.Default.Encode(message);
            this._notification.Error(encoded);
            return View();
        }

        return RedirectToAction("All", "Users");
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
            var encoded = HtmlEncoder.Default.Encode(userModel?.Error?.Message ?? GlobalMessages.GlobalError);
            this._notification.Error(encoded);
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
        if (result.IsFailure)
        {
            var message = result?.Error?.Message ?? GlobalMessages.GlobalError;
            var encoded = HtmlEncoder.Default.Encode(message);
            this._notification.Error(encoded);
            return View(userModel);
        }
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
    
    private SortCollection SetSortParam(string? orderBy)
    {
        var sortedCollection = new SortCollection();
        sortedCollection.AddColumn("UserName", "UserName desc");
        sortedCollection.AddColumn("Email", "Email desc");
        sortedCollection.AddColumn("FirstName", "FirstName desc");
        sortedCollection.AddColumn("LastName", "LastName desc");
        sortedCollection.AddColumn("IsConfirmed", "IsConfirmed desc");
        
        switch (orderBy ?? string.Empty)
        {
            case "UserName desc":
                sortedCollection.Update("UserName", "UserName", "fa-solid fa-arrow-up-wide-short");
                break;
            case "Email":
                sortedCollection.Update("Email", "Email desc", "fa-solid fa-arrow-up-short-wide");
                break;
            case "Email desc":
                sortedCollection.Update("Email", "Email", "fa-solid fa-arrow-up-wide-short");
                break;
            case "FirstName":
                sortedCollection.Update("FirstName", "FirstName desc", "fa-solid fa-arrow-up-short-wide");
                break;
            case "FirstName desc":
                sortedCollection.Update("FirstName", "FirstName", "fa-solid fa-arrow-up-wide-short");
                break;
            case "LastName":
                sortedCollection.Update("LastName", "LastName desc", "fa-solid fa-arrow-up-short-wide");
                break;
            case "LastName desc":
                sortedCollection.Update("LastName", "LastName", "fa-solid fa-arrow-up-wide-short");
                break;
            case "IsConfirmed":
                sortedCollection.Update("IsConfirmed", "IsConfirmed desc", "fa-solid fa-arrow-up-short-wide");
                break;
            case "IsConfirmed desc":
                sortedCollection.Update("IsConfirmed", "IsConfirmed", "fa-solid fa-arrow-up-wide-short");
                break;
            default:
                sortedCollection.Update("UserName", "UserName desc", "fa-solid fa-arrow-up-short-wide");
                break;
        }

        return sortedCollection;
    }
}