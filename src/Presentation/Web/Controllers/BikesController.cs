using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System;
using Web.Models.Bikes;
using Web.Models.BikeTypes;

namespace Web.Controllers
{
    [Authorize(Roles = "User,Manager")]
    public class BikesController : AuthorizationController
    {
        public BikesController(
            IHttpContextAccessor httpContextAccessor, 
            IHttpClientFactory httpClientFactory, 
            IConfiguration configuration) 
            : base(httpContextAccessor, httpClientFactory, configuration)
        {
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var token = this.GetJwtToken();
            if (token is null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            var query = new Dictionary<string, string>() { { "UserId", base.CurrentUserId() } };
            var bikesResponse = await this.GetAsync<IEnumerable<BikeViewModel>>("/api/Bikes", token, query);
            if (bikesResponse.IsFailure)
            {
                return View();
            }

            return View(bikesResponse.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var token = this.GetJwtToken();
            if (token is null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            var bikeTypes = await this.GetAsync<IEnumerable<BikeTypeViewModel>>("/api/BikeTypes", token);
            if (bikeTypes.IsFailure)
            {
                return View();
            }

            var bikeModel = new BikeInputModel() 
            { 
                UserId = this.CurrentUserId(), 
                BikeTypes = bikeTypes.Value!
            };

            return View(bikeModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BikeInputModel bikeModel)
        {
            var token = this.GetJwtToken();
            if (token is null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            if (!ModelState.IsValid)
            {
                var bikeTypes = await this.GetAsync<IEnumerable<BikeTypeViewModel>>("/api/BikeTypes", token);
                bikeModel.BikeTypes = bikeTypes.Value!;
                return View(bikeModel);
            }

            if (bikeModel.UserId != this.CurrentUserId())
            {
                return View();
            }

            var query = new Dictionary<string, string>() { { "Id", bikeModel.BikeTypeId } };
            var bikeTypeExists = await this.GetAsync<bool>("/api/BikeTypes/Exists", token, query);
            if (bikeTypeExists.IsFailure 
                || (bikeTypeExists.IsSuccess && bikeTypeExists.Value == false))
            {
                return View();
            }

            await this.PostAsync<BikeInputModel,Guid>("/api/Bikes", bikeModel, token);
            return RedirectToAction("All", "Bikes");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var token = this.GetJwtToken();
            if (token is null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            var bike = await this.GetAsync<BikeInputModel>($"/api/Bikes/{id}", token);
            if (bike.IsFailure)
            {
                return View();
            }

            var model = bike.Value;
            if (model is null) 
            {
                return View();
            }

            var types = await this.GetAsync<IEnumerable<BikeTypeViewModel>>("/api/BikeTypes", token);
            model.BikeTypes = types.Value!;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BikeInputModel bikeModel)
        {
            var token = this.GetJwtToken();
            if (token is null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            if (!ModelState.IsValid)
            {
                var types = await this.GetAsync<IEnumerable<BikeTypeViewModel>>("/api/BikeTypes", token);
                bikeModel.BikeTypes = types.Value!;
                return View(bikeModel);
            }

            var result = await this.PutAsync<BikeInputModel>("/api/Bikes", bikeModel, token);
            return RedirectToAction("All", "Bike");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var token = this.GetJwtToken();
            if (token is null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            await this.DeleteAsync("/api/Bikes/", Guid.Parse(id), token);
            return RedirectToAction("All", "Bike");
        }
    }
}
