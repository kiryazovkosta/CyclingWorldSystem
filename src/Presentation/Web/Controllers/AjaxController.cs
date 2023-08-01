using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using Web.Extensions;
using Web.Models.GpxFiles;
using Web.Models.Waypoints;

namespace Web.Controllers;

using Common.Constants;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Models;
using Models.Likes;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;

public class AjaxController : AuthorizationController
{
    public AjaxController(
        IHttpContextAccessor httpContextAccessor, 
        IHttpClientFactory httpClientFactory, 
        IConfiguration configuration) 
        : base(httpContextAccessor, httpClientFactory, configuration)
    {
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProcessGps([FromForm] FilesListModel model)
    {
        {
            var token = this.GetJwtToken();
            if (token is null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            if (!model.Files.Any())
            {
                throw new NullReferenceException($"Collection must contains at least one file");
            }

            var gpxFile = model.Files.First();
            var xml = await gpxFile.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(xml))
            {
                throw new ArgumentException($"The provided Gpx file has invalid content");
            }

            var request = new ProcessGpxFileModel(EncodeTo64(xml));
            var response = await this.PostAsync<ProcessGpxFileModel, GpxFileViewModel>("/api/GpxFiles", request, token);
            if (response.IsFailure)
            {
                throw new Exception("There is a problem when processing the selected Gpx file.");
            }
            
            return Ok(response.Value);
        }

    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProcessImages([FromForm] FilesListModel model)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        if (!model.Files.Any())
        {
            throw new NullReferenceException($"Collection must contains at least one file");
        }
        
        var imageModel = new PicturesInputModel(model.Files);
        var result = await this.PostOnlyFilesAsync(
            "api/Images/Multi", 
            imageModel, 
            token);
        if (result.IsFailure)
        {
            throw new ArgumentException(result?.Error?.Message ?? GlobalMessages.GlobalError);
        }
       
        return Ok(result.Value);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GetCoordinates([FromForm] string activityId)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }
        
        var response = await this.GetAsync<IEnumerable<WaypointCoordinate>>(
                $"api/Waypoints/Coordinates/{activityId}", 
                token);
        if (response.IsFailure)
        {
            throw new Exception("There is a problem with fetching track for activity");
        }

        var coordinates = response.Value!.ToArray();
        return Ok(coordinates);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GetStartCoordinate([FromForm] string activityId)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }
        
        var response = await this.GetAsync<WaypointCenterCoordinate>(
            $"api/Waypoints/Center/{activityId}", 
            token);
        if (response.IsFailure)
        {
            throw new Exception("There is a problem with fetching track for activity");
        }

        var coordinates = response.Value!;
        return Ok(coordinates);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LikeActivity([FromForm] string activityId)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        var like = new ActivityLikeModel(Guid.Parse(activityId), Guid.Parse(this.CurrentUserId()));
        var response = await this.PostAsync<ActivityLikeModel, bool>("/api/ActivityLikes", like, token);
        if (response.IsFailure)
        {
            throw new Exception("There is a problem when like the selected activity!");
        }
        
        var result = new { IsSuccess = true};
        return Ok(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DislikeActivity([FromForm] string activityId)
    {
        var token = this.GetJwtToken();
        if (token is null)
        {
            return RedirectToAction("LogIn", "Account");
        }

        var dislike = new ActivityDislikeModel(Guid.Parse(activityId), Guid.Parse(this.CurrentUserId()));
        var response = await this.PostAsync<ActivityDislikeModel, bool>("/api/ActivityLikes/Dislike", dislike, token);
        if (response.IsFailure)
        {
            throw new Exception(response?.Error?.Message ?? GlobalMessages.GlobalError);
        }
        
        var result = new { IsSuccess = true};
        return Ok(result);
    }

    private static string EncodeTo64(string toEncode)
    {
        return Convert.ToBase64String(
        Encoding.UTF8.GetBytes(toEncode));
    }
}
