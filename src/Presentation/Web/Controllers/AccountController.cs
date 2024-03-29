﻿

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
using Models;
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

        var responseModel = new ResultMessageModel();
        if (result.IsFailure)
        {
            responseModel.Error = result?.Error?.Message ?? GlobalMessages.GlobalError;
        }
        else
        {
            responseModel.Success =  "Thank you for confirming your email.";
        }
        
        return RedirectToAction("Index", "Home", responseModel);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResendEmailConfirmation()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResendEmailConfirmation(string email)
    {
        var model = new ResendEmailConfirmationInputModel(email);
        var result =
            await this.PostAsync<ResendEmailConfirmationInputModel, bool>("api/Accounts/ResendConfirmEmail", model);

        var responseModel = new ResultMessageModel();
        if (result.IsFailure)
        {
            responseModel.Error = result?.Error?.Message ?? GlobalMessages.GlobalError;
        }
        else
        {
            responseModel.Success =  "Verification email sent. Please check your email.";
        }
        
        return RedirectToAction("Index", "Home", responseModel);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(string userName)
    {
        var model = new ForgotPasswordInputModel() { UserName =  userName };
        var result =
            await this.PostAsync<ForgotPasswordInputModel, bool>("api/Accounts/ForgotPassword", model);

        var responseModel = new ResultMessageModel();
        if (result.IsFailure)
        {
            responseModel.Error = result?.Error?.Message ?? GlobalMessages.GlobalError;
        }
        else
        {
            responseModel.Success =  "Password reset verification email sent. Please check your email.";
        }
        
        return RedirectToAction("Index", "Home", responseModel);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPassword([FromQuery] string UserId, [FromQuery] string Code)
    {
        var model = new ResetPasswordInputModel() 
        {
            UserId = UserId,
            Code = Code
        };

        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPasswordInputModel model)
    {
        var result =
            await this.PostAsync<ResetPasswordInputModel, bool>("api/Accounts/ResetPassword", model);

        var responseModel = new ResultMessageModel();
        if (result.IsFailure)
        {
            responseModel.Error = result?.Error?.Message ?? GlobalMessages.GlobalError;
        }
        else
        {
            responseModel.Success =  "Password is changed successfully.";
        }
        
        return RedirectToAction("Index", "Home", responseModel);
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