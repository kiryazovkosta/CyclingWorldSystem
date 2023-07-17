using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class BikesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
