using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Manage.Controllers
{
    public class ManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
