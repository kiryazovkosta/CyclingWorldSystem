using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Manage.Controllers
{
    using Microsoft.AspNetCore.Authorization;

    [Authorize(Roles = "Administrator")]
    public class ManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
