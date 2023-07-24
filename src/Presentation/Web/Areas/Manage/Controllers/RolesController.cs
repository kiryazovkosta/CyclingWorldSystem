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

}