

namespace Web.Controllers;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using AspNetCoreHero.ToastNotification.Abstractions;
using Common.Constants;
using Microsoft.AspNetCore.WebUtilities;
using Web.Models.Authorization;

[Authorize]
public class AccountController : AuthorizationController
{

    private readonly INotyfService _notification;

    public AccountController(
        IHttpContextAccessor httpContextAccessor, 
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration, 
        INotyfService notification) 
        : base(httpContextAccessor, httpClientFactory, configuration)
    {
        this._notification = notification ?? throw new ArgumentNullException(nameof(notification));
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        var model = new LogInUserInputModel();
        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LogInUserInputModel model)
    {
        var result =
            await this.PostAsync<LogInUserInputModel, LogInResponse>(
                "api/Accounts/LogIn", model);
        if (result.IsFailure)
        {
            var message = result?.Error?.Message ?? GlobalMessages.GlobalError;
            this._notification.Error(message);
            return View();
        }

        JwtSecurityToken token = GetJwtTokenData(result.Value);
        await SetAuthenticationCookie(token);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        var model = new RegisterUserInputModel();
        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterUserInputModel model)
    {
        var result = await this.PostMultipartAsync<RegisterUserInputModel, Guid>("api/Accounts/Register", model);
        if (result.IsFailure)
        {
            var message = result?.Error?.Message ?? GlobalMessages.GlobalError;
            var encoded = HtmlEncoder.Default.Encode(message);
            this._notification.Error(encoded);
            return View();
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string UserId, [FromQuery] string Code)
    {
        var confirm = new ConfirmEmailInputModel(UserId, Code);
        var result =
             await this.PostAsync<ConfirmEmailInputModel, bool>("api/Accounts/ConfirmEmail", confirm);
        if (result.IsFailure)
        {
            CookieOptions options = new CookieOptions() 
            { 
                Expires = DateTime.Now.AddHours(5), 
                HttpOnly = true, 
                IsEssential = true
            };
            Response.Cookies.Append("ErrorMessage", result?.Error?.Message ?? GlobalMessages.GlobalError, options);
        }
        else
        {
            CookieOptions options = new CookieOptions() 
            { 
                HttpOnly = true,
                IsEssential = true,
                Secure = false,
                SameSite = SameSiteMode.None,
                Domain = "localhost",
                Expires = DateTime.UtcNow.AddDays(14)
            };
            Response.Cookies.Append("s-ss-sss", "Thank you for confirming your email.", options);
        }
        
        return RedirectToAction("Index", "Home");
    }

    [HttpGet] 
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpGet] 
    public IActionResult ResetPassword()
    {
        return View();
    }

    private JwtSecurityToken GetJwtTokenData(LogInResponse? result)
    {
        var jwtToken = result?.Token ?? string.Empty;
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            IsEssential = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Domain = "localhost",
            Expires = DateTime.UtcNow.AddMinutes(119)
        };

        var cookieName = base.Configuration["Backend:JwtTokenName"] ?? string.Empty;
        HttpContextAccessor.HttpContext?.Response?.Cookies.Append(cookieName, jwtToken!, cookieOptions);
        var token = new JwtSecurityToken(jwtEncodedString: jwtToken);
        return token;
    }

    private async Task SetAuthenticationCookie(JwtSecurityToken token)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, token.Claims.First(c => c.Type == "username").Value),
            new Claim("email", token.Claims.First(c => c.Type == "email").Value),
            new Claim(ClaimTypes.NameIdentifier, token.Claims.First(c => c.Type == "identifier").Value),
            new Claim("fullname", token.Claims.First(c => c.Type == "fullname").Value),
            new Claim("avatar", token.Claims.First(c => c.Type == "avatar").Value)
        };

        var roles = token.Claims.Where(c => c.Type == "role").ToList();
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Value));
        }

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }
}