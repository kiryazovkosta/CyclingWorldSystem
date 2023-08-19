namespace Web.Areas.Manage.Controllers;

using AspNetCoreHero.ToastNotification.Abstractions;
using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Web.Controllers;

[Authorize(Roles = "Administrator")]
public class RolesController : AuthorizationController
{
    private readonly INotyfService _notification;
    public RolesController(
        IHttpContextAccessor httpContextAccessor, 
        IHttpClientFactory httpClientFactory, 
        IConfiguration configuration,
        INotyfService notification) 
        : base(httpContextAccessor, httpClientFactory, configuration)
    {
        this._notification = notification ?? throw new ArgumentNullException(nameof(notification));
    }

    public async Task<IActionResult> All()
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        var rolesResponse = await this.GetAsync<IEnumerable<RoleFullViewModel>>("api/Roles", token);
        if (rolesResponse.IsFailure)
        {
            var message = rolesResponse?.Error?.Message ?? GlobalMessages.GlobalError;
            this._notification.Error(message);
            return RedirectToAction("Index", "Management");
        }
        
        return View(rolesResponse.Value);
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        var roleModel = new RoleInputModel() { UserId = this.CurrentUserId() };
        return View(roleModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(RoleInputModel roleModel)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }
    
        if (!ModelState.IsValid)
        { 
            return View(roleModel);
        }
    
        if (roleModel.UserId != this.CurrentUserId())
        {
            var message = GlobalMessages.CredentialsMismatch;
            this._notification.Error(message);
            return View();
        }
    
        var createRoleResponse = 
            await this.PostAsync<RoleInputModel, Guid>("/api/Roles", roleModel, token);
        if (createRoleResponse.IsFailure)
        {
            var message = createRoleResponse?.Error?.Message ?? GlobalMessages.GlobalError;
            this._notification.Error(message);
            return View(roleModel);
        }
        
        return RedirectToAction("All", "Roles");
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        var roleModelResponse = await this.GetAsync<RoleInputModel>($"/api/Roles/{id}", token);
        if (roleModelResponse.IsFailure)
        {
            var message = roleModelResponse?.Error?.Message ?? GlobalMessages.GlobalError;
            this._notification.Error(message);
            return View();
        }

        var role = roleModelResponse.Value!;
        role.UserId = this.CurrentUserId();
        return View(role);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(RoleInputModel roleModel)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        if (!ModelState.IsValid)
        { 
            return View(roleModel);
        }
        
        var editRoleResponse = await this.PutAsync<RoleInputModel>("/api/Roles", roleModel, token);
        if (editRoleResponse.IsFailure)
        {
            var message = editRoleResponse?.Error?.Message ?? GlobalMessages.GlobalError;
            this._notification.Error(message);
            return View(roleModel);
        }
        
        return RedirectToAction("All", "Roles");
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        await this.DeleteAsync("/api/Roles/", Guid.Parse(id), token);
        return RedirectToAction("All", "Roles");
    }

}