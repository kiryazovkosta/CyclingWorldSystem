using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using Web.Extensions;
using Web.Models.GpxFiles;
using Web.Models.Waypoints;

namespace Web.Controllers;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Models;
using Newtonsoft.Json;

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
    // [Produces("application/json")]
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
            throw new ArgumentException(result.Error);
        }
        
        //this.TempData["Pictures"] = result.Value!;
        return Ok(result.Value);
    }

    private static string EncodeTo64(string toEncode)
    {

        byte[] toEncodeAsBytes = UTF8Encoding.UTF8.GetBytes(toEncode);
        string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
        return returnValue;
    }
}
