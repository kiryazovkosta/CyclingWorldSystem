using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using Web.Models.Authorization;
using Web.Models.BikeTypes;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Web.Controllers
{
	public class TestController : AuthorizationController
	{
		public TestController(
			IHttpContextAccessor httpContextAccessor, 
			IHttpClientFactory httpClientFactory, 
			IConfiguration configuration) 
			: base(httpContextAccessor, httpClientFactory, configuration)
		{
		}

		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> Index()
		{

			var token = this.GetJwtToken();
			if (token is null)
			{
				return RedirectToAction("LogIn", "Account");
			}
			var bikesResponse = await this.GetAsync<IEnumerable<BikeTypeViewModel>>("/api/BikeTypes", token);
			if (bikesResponse.IsFailure)
			{
				return View();
			}

			return View(bikesResponse.Value);
		}
	}
}
