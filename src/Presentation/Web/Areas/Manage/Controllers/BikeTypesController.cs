using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Manage.Controllers
{
    public class BikeTypesController : Controller
    {
        public IActionResult All()
        {
            return View();
        }
    }
}
