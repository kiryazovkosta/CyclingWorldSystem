using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Manage.Controllers
{
    using AspNetCoreHero.ToastNotification.Abstractions;
    using Common.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Models.Nomenclatures;
    using Web.Controllers;

    [Authorize(Roles = "Administrator")]
    public class BikeTypesController : AuthorizationController
    {

        private readonly INotyfService _notification;
        public BikeTypesController(
            IHttpContextAccessor httpContextAccessor, 
            IHttpClientFactory httpClientFactory, 
            IConfiguration configuration,
            INotyfService notification) 
            : base(httpContextAccessor, httpClientFactory, configuration)
        {
            this._notification = notification ?? throw new ArgumentNullException(nameof(notification));
        }
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var token = this.GetJwtToken();
            if (token is null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            var rolesResponse = await this.GetAsync<IEnumerable<BikeTypeAdminViewModel>>("api/BikeTypes", token);
            if (rolesResponse.IsFailure)
            {
                return RedirectToAction("Index", "Management");
            }
        
            return View(rolesResponse.Value);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var token = this.GetJwtToken();
            if (token is null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(BikeTypeInputModel model)
        {
            var token = this.GetJwtToken();
            if (token is null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            
            if (!ModelState.IsValid)
            {
                this._notification.Error("Error");
                return View(model);
            }
        
            var result = await this.PostAsync<BikeTypeInputModel, Guid>("/api/BikeTypes", model, token);
            if (result.IsFailure)
            {
                this._notification.Error(result?.Error?.Message ?? GlobalMessages.GlobalError);
                return View(model);
            }
            
            return RedirectToAction(nameof(this.All));
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var token = this.GetJwtToken();
            if (token is null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            var result = await this.GetAsync<BikeTypeUpdateInputModel>($"api/BikeTypes/{id}", token);
            if (result.IsFailure)
            {
                return View(id);
            }

            return View(result.Value!);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(BikeTypeUpdateInputModel model)
        {
            var token = this.GetJwtToken();
            if (token is null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var result = await this.PutAsync<BikeTypeUpdateInputModel>("api/BikeTypes", model, token);
            if (result.IsFailure)
            {
                return View(model);
            }
            
            return RedirectToAction(nameof(this.All));
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var token = this.GetJwtToken();
            if (token is null)
            {
                return RedirectToAction("LogIn", "Account");
            }
            
            await this.DeleteAsync("/api/BikeTypes/", Guid.Parse(id), token);
            return RedirectToAction(nameof(this.All));
        }
        
        [HttpPost]
        public IActionResult Undelete(string id)
        {
            //await this.bikeTypeService.UndeleteAsync(id);
            return RedirectToAction(nameof(this.All));
        }


    }
}
