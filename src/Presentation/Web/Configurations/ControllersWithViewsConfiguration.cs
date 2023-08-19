namespace Web.Configurations;

public static class ControllersWithViewsConfiguration
{
    public static IServiceCollection AddControllersWithViewsConfig(this IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddSignalR().AddMessagePackProtocol();
        services.AddServerSideBlazor();
        
        return services;
    }
}
