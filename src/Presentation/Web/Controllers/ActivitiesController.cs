using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ActivitiesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
