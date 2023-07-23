// ------------------------------------------------------------------------------------------------
//  <copyright file="ICloudinaryService.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Interfaces;

using Microsoft.AspNetCore.Http;

public interface ICloudinaryService
{
    bool IsFileValid(IFormFile? photoFile);

    Task<string> UploadAsync(IFormFile? file);

    Task<string> UploadArrayAsync(byte[] file);

    Task<List<string>> UploadMultiAsync(List<IFormFile> files);
}