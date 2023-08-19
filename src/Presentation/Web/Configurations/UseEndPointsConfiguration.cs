using Web.Hubs;

namespace Web.Configurations;

public static class UseEndPointsConfiguration
{
    public static IApplicationBuilder UseEndPoints(this IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapAreaControllerRoute(
                name: "Manage", 
                areaName: "Manage", 
                pattern: "Manage/{controller=Management}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(
                name: "default", 
                pattern: "{controller=Home}/{action=Index}/{id?}");

            endpoints.MapHub<ActivityHub>("/activityhub");

            endpoints.MapBlazorHub();
        });
        return app;
    }
}
