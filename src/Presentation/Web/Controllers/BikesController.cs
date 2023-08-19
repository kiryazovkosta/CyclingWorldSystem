using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models.Bikes;
using Web.Models.BikeTypes;

namespace Web.Controllers
{
    using AspNetCoreHero.ToastNotification.Abstractions;
    using Common.Constants;

    [Authorize(Roles = "User,Manager")]
    public class BikesController : AuthorizationController
    {
        private readonly INotyfService _notification;
        
        public BikesController(
            IHttpContextAccessor httpContextAccessor, 
            IHttpClientFactory httpClientFactory, 
            IConfiguration configuration,
            INotyfService notification) 
            : base(httpContextAccessor, httpClientFactory, configuration)
        {
            ArgumentNullException.ThrowIfNull(notification);
            this._notification = notification;
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
                var errorMessage = bikesResponse?.Error?.Message ?? GlobalMessages.GlobalError;
                this._notification.Error(errorMessage);
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

            var bikeTypesResponse = await this.GetAsync<IEnumerable<BikeTypeViewModel>>("/api/BikeTypes", token);
            if (bikeTypesResponse.IsFailure)
            {
                var errorMessage = bikeTypesResponse?.Error?.Message ?? GlobalMessages.GlobalError;
                this._notification.Error(errorMessage);
                return View();
            }

            var bikeModel = new BikeInputModel() 
            { 
                Id = Guid.Empty.ToString(),
                UserId = this.CurrentUserId(), 
                BikeTypes = bikeTypesResponse.Value!
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
                var errorMessage = GlobalMessages.CredentialsMismatch;
                this._notification.Error(errorMessage);
                return View(bikeModel);
            }

            var query = new Dictionary<string, string>() { { "Id", bikeModel.BikeTypeId } };
            var bikeTypeExists = await this.GetAsync<bool>("/api/BikeTypes/Exists", token, query);
            if (bikeTypeExists.IsFailure 
                || (bikeTypeExists.IsSuccess && bikeTypeExists.Value == false))
            {
                var errorMessage = bikeTypeExists?.Error?.Message ?? GlobalMessages.GlobalError;
                this._notification.Error(errorMessage);
                var bikeTypes = await this.GetAsync<IEnumerable<BikeTypeViewModel>>("/api/BikeTypes", token);
                bikeModel.BikeTypes = bikeTypes.Value!;
                return View(bikeModel);
            }

            var createResult =  await this.PostAsync<BikeInputModel, Guid>("/api/Bikes", bikeModel, token);
            if (createResult.IsFailure)
            {
                var errorMessage = createResult?.Error?.Message ?? GlobalMessages.GlobalError;
                this._notification.Error(errorMessage);
                var bikeTypes = await this.GetAsync<IEnumerable<BikeTypeViewModel>>("/api/BikeTypes", token);
                bikeModel.BikeTypes = bikeTypes.Value!;
                return View(bikeModel);
            }
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
                var errorMessage = bike?.Error?.Message ?? GlobalMessages.GlobalError;
                this._notification.Error(errorMessage);
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

            var bikeUpdateResponse = await this.PutAsync<BikeInputModel>("/api/Bikes", bikeModel, token);
            if (bikeUpdateResponse.IsFailure)
            {
                var errorMessage = bikeUpdateResponse?.Error?.Message ?? GlobalMessages.GlobalError;
                this._notification.Error(errorMessage);
                var types = await this.GetAsync<IEnumerable<BikeTypeViewModel>>("/api/BikeTypes", token);
                bikeModel.BikeTypes = types.Value!;
                return View(bikeModel);
            }
            
            return RedirectToAction("All", "Bikes");
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
            return RedirectToAction("All", "Bikes");
        }
    }
}
