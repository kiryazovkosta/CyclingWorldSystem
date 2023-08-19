using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Diagnostics;
using NuGet.Protocol;

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

    public IActionResult Index(ResultMessageModel? model)
    {
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int statusCode)
    {
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandlerPathFeature is not null)
        {
            this._logger.LogError(exceptionHandlerPathFeature.Error, exceptionHandlerPathFeature.Path);
        }
        
        if (statusCode == 400)
        {
            return View("BadRequestError");
        }
        if (statusCode == 401)
        {
            return View("UnauthorizedError");
        }
        
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}