using Microsoft.AspNetCore.Mvc;
using Web.Models.Activities;

namespace Web.Controllers
{
    using AspNetCoreHero.ToastNotification.Abstractions;
    using Common.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Models.Bikes;
    using Web.Hubs;

    [Authorize(Roles = "User,Manager")]
    public class ActivitiesController : AuthorizationController
    {
        private readonly IHubContext<ActivityHub> _activityHub;
        private readonly INotyfService _notification;

        public ActivitiesController(
            IHttpContextAccessor httpContextAccessor, 
            IHttpClientFactory httpClientFactory, 
            IConfiguration configuration,
            IHubContext<ActivityHub> activityHub,
            INotyfService notification) 
            : base(httpContextAccessor, httpClientFactory, configuration)
        {
            ArgumentNullException.ThrowIfNull(activityHub);
            ArgumentNullException.ThrowIfNull(notification);
            
            this._activityHub = activityHub;
            this._notification = notification;
        }

        [HttpGet]
        public async Task<IActionResult> All(int? pageSize, int? pageNumber, string? searchString)
        {
            var token = this.GetJwtToken();
            if (token is null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            var searchCriteria = ViewData["CurrentFilter"];

            var inputModel = new QueryParameterInputModel()
            {
                PageSize = pageSize ?? 6,
                PageNumber = pageNumber ?? 1,
                SearchBy = searchString
            };

            var parameters = new Dictionary<string, string>()
            {
                { "PageSize", inputModel.PageSize.ToString() },
                { "PageNumber", inputModel.PageNumber.ToString() },
                { "SearchBy", inputModel.SearchBy ?? string.Empty }
            };
            var activitiesResponse = 
                await this.GetAsync<PagedActivityDataViewModel>("api/Activities", token, parameters);
            if (activitiesResponse.IsFailure)
            {
                var message = activitiesResponse?.Error?.Message ?? GlobalMessages.GlobalError;
                this._notification.Error(message);
                return View();
            }
            
            return View(activitiesResponse.Value!);
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            var token = this.GetJwtToken();
            if (token is null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            var userId = this.CurrentUserId();
            var mineActivities 
                = await this.GetAsync<IEnumerable<MyActivityViewModel>>($"api/Activities/GetMine/{userId}", token);
            if (mineActivities.IsFailure)
            {
                var message = mineActivities?.Error?.Message ?? GlobalMessages.GlobalError;
                this._notification.Error(message);
                return View();
            }
            
            return View(mineActivities.Value);
        }
        
        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var token = this.GetJwtToken();
            if (token is null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            var activityResponse = await this.GetAsync<ActivityViewModel>($"api/Activities/{id}", token);
            if (activityResponse.IsFailure)
            {
                var message = activityResponse?.Error?.Message ?? GlobalMessages.GlobalError;
                this._notification.Error(message);
                 return View();
            }

            var model = activityResponse.Value!;
            model.CurrentUserId = Guid.Parse(this.CurrentUserId());
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var token = this.GetJwtToken();
            if (token is null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            var userId = this.CurrentUserId();
            var query = new Dictionary<string, string>() { { "UserId", base.CurrentUserId() } };
            var bikesResponse = await this.GetAsync<IEnumerable<SimpleBikeViewModel>>("/api/Bikes", token, query);
            if (bikesResponse.IsFailure)
            {
                var message = bikesResponse?.Error?.Message ?? GlobalMessages.GlobalError;
                this._notification.Error(message);
                return View();
            }
            
            var model = new ActivityInputModel() 
            { 
                UserId = userId,
                Bikes = bikesResponse.Value!
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ActivityInputModel model)
        {
            var token = this.GetJwtToken();
            if (token is null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            if (!string.IsNullOrWhiteSpace(model.PicturesList))
            {
                var pictures = model.PicturesList.Split(";").ToList();
                model.Pictures = pictures;
            }

            var response = await this.PostAsync<ActivityInputModel, Guid>(
                "/api/Activities", model, token);
            if (response.IsFailure) 
            {
                var errorMessage = response?.Error?.Message ?? GlobalMessages.GlobalError;
                this._notification.Error(errorMessage);
                return View();
            }

            var message = $"{this.User?.Identity?.Name} create activity with name {model.Title}";
            await this._activityHub.Clients.All.SendAsync("NotifyActivityCreateAsync", message);
            return RedirectToAction("All", "Activities");
        }
    }
}
