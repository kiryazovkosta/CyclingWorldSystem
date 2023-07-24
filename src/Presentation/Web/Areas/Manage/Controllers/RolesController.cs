namespace Web.Areas.Manage.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Web.Controllers;

[Authorize(Roles = "Administrator")]
public class RolesController : AuthorizationController
{
    public RolesController(
        IHttpContextAccessor httpContextAccessor, 
        IHttpClientFactory httpClientFactory, 
        IConfiguration configuration) 
        : base(httpContextAccessor, httpClientFactory, configuration)
    {
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
            return View();
        }
    
        await this.PostAsync<RoleInputModel, Guid>("/api/Roles", roleModel, token);
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

        var userModel = await this.GetAsync<RoleInputModel>($"/api/Roles/{id}", token);
        if (userModel.IsFailure)
        {
            return View();
        }

        var role = userModel.Value!;
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
        
        var result = await this.PutAsync<RoleInputModel>("/api/Roles", roleModel, token);
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