namespace Web.Configurations;

public static class UseEnvironmentConfiguration
{
    public static WebApplication UseEnvironment(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error/500");
            app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");
            app.UseHsts();
        }

        return app;
    }
}
