// ------------------------------------------------------------------------------------------------
//  <copyright file="AddNotificationsConfiguration.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Configurations;

using AspNetCoreHero.ToastNotification;

public static class AddNotificationsConfiguration
{
    public static IServiceCollection AddToastNotifications(this IServiceCollection serviceCollection) 
    {
        serviceCollection.AddNotyf(config=> { 
            config.DurationInSeconds = 5;
            config.IsDismissable = true;
            config.Position = NotyfPosition.TopRight; 
            config.HasRippleEffect = true; });
        return serviceCollection;
    }
}