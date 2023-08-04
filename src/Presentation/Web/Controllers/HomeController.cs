using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

using AspNetCoreHero.ToastNotification.Abstractions;

public class HomeController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<HomeController> _logger;
    private readonly INotyfService _notification;

    public HomeController(IHttpContextAccessor httpContextAccessor, ILogger<HomeController> logger,
        INotyfService notification)
    {
        this._httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this._notification = notification ?? throw new ArgumentNullException(nameof(notification));
    }

    public IActionResult Index()
    {
        string name = this._httpContextAccessor.HttpContext.Request.Cookies["s-ss-sss"];

        
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}