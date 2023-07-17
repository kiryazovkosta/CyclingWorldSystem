using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Manage.Controllers
{
    public class BikeTypesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
