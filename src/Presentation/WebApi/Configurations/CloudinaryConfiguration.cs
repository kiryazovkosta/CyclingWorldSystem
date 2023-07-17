// ------------------------------------------------------------------------------------------------
//  <copyright file="CloudinaryConfiguration.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace WebApi.Extensions;

using CloudinaryDotNet;

public static class CloudinaryConfiguration
{
    public static IServiceCollection AddCloudinary(this IServiceCollection services, IConfiguration configuration)
    {
        var cloudinaryCredentials = new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]);
        var cloudinary = new Cloudinary(cloudinaryCredentials);
        services.AddSingleton(cloudinary);
        return services;
    }
}