using Microsoft.AspNetCore.Mvc;
using System.Text;
using Web.Extensions;
using Web.Models.Activities;
using Web.Models.GpxFiles;

namespace Web.Controllers
{
    using Models.Bikes;
    using Models.Waypoints;

    public class ActivitiesController : AuthorizationController
    {
        public ActivitiesController(
            IHttpContextAccessor httpContextAccessor, 
            IHttpClientFactory httpClientFactory, 
            IConfiguration configuration) 
            : base(httpContextAccessor, httpClientFactory, configuration)
        {
        }

        public IActionResult All()
        {
            return View();
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
            
            return RedirectToAction("All", "Activities");
        }


    }
}
