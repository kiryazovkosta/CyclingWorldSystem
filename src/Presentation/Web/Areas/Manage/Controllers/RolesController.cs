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
}