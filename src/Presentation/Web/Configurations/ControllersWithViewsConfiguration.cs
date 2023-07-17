namespace Web.Configurations;

public static class ControllersWithViewsConfiguration
{
    public static IServiceCollection AddControllersWithViewsConfig(this IServiceCollection services)
    {
        services
            .AddControllersWithViews();

        return services;
    }
}
