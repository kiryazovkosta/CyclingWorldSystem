namespace Web.Configurations;

public static class UseCookiePolicyConfiguration
{
    public static IApplicationBuilder UseCookiePolicyConfig(this IApplicationBuilder app)
    {
        var cookiePolicyOptions = new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.Strict,
            HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
            Secure = CookieSecurePolicy.None
        };

        app.UseCookiePolicy(cookiePolicyOptions);
        return app;
    }
}
