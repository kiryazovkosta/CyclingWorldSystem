using Microsoft.AspNetCore.Authentication.Cookies;

namespace Web.Configurations
{
    public static class AuthenticationConfiguration
    {
        public static IServiceCollection AddAuthenticationConfig(this IServiceCollection services)
        {
            services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.LoginPath = "/Account/LogIn";
                    options.LogoutPath = "/Account/LogOut";
                    options.AccessDeniedPath = "/";
                    options.Cookie.Name = "MySessionCookie";
                    options.SlidingExpiration = true;
                });

            return services;
        }
    }
}
