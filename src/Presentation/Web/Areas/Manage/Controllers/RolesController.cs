namespace Web.Areas.Manage.Controllers;

using Microsoft.AspNetCore.Mvc;
using Web.Controllers;

public class RolesController : AuthorizationController
{
    public RolesController(
        IHttpContextAccessor httpContextAccessor, 
        IHttpClientFactory httpClientFactory, 
        IConfiguration configuration) 
        : base(httpContextAccessor, httpClientFactory, configuration)
    {
    }

    public IActionResult All()
    {
        return View();
    }
}