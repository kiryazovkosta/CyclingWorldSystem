namespace Application.Entities.Images.Models;

using Microsoft.AspNetCore.Http;

public sealed record CreateImageRequest(IFormFile File);