// ------------------------------------------------------------------------------------------------
//  <copyright file="CloudinaryService.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Services;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Common.Constants;
using Exceptions;
using Interfaces;
using Microsoft.AspNetCore.Http;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary cloudinary;

    public CloudinaryService(Cloudinary cloudinary)
    {
        this.cloudinary = cloudinary ?? throw new ArgumentNullException(nameof(cloudinary));
    }

    public bool IsFileValid(IFormFile? photoFile)
    {
        if (photoFile is null)
        {
            return false;
        }

        if (GlobalConstants.Cloudinary.ValidImagesTypes.Contains(photoFile.ContentType) == false)
        {
            return false;
        }

        return true;
    }

    public async Task<string> UploadAsync(IFormFile? file)
    {
        if (file is null || this.IsFileValid(file) == false)
        {
            throw new CloudinaryUploadException(GlobalMessages.Cloudinary.ExceptionMessage);
        }

        var url = " ";
        byte[] fileBytes;
        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            fileBytes = stream.ToArray();
        }

        using (var uploadStream = new MemoryStream(fileBytes))
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, uploadStream),
            };
            var result = await this.cloudinary.UploadAsync(uploadParams);

            url = result.Url?.AbsoluteUri ?? string.Empty;
        }

        return url;
    }

    // public async Task<string> UploadArrayAsync(byte[] file)
    // {
    //     if (file == null || file.Length == 0)
    //     {
    //         throw new CloudinaryUploadException(GlobalMessages.Cloudinary.ExceptionMessage);
    //     }
    //
    //     var bytes = file;
    //     var url = " ";
    //
    //     using var uploadStream = new MemoryStream(bytes);
    //     var uploadParams = new ImageUploadParams()
    //     {
    //         File = new FileDescription(file.ToString(), uploadStream),
    //     };
    //     var result = await this.cloudinary.UploadAsync(uploadParams);
    //
    //     url = result.Url.AbsoluteUri;
    //
    //     return url;
    // }
    
    public async Task<List<string>> UploadMultiAsync(List<IFormFile> files)
    {
        var result = new List<string>();
        foreach (var file in files)
        {
            var url = await this.UploadAsync(file);
            result.Add(url);
        }

        return result;
    } 
}