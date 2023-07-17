using Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddHttpClientExtension(builder.Configuration)
	.AddControllersWithViewsConfig()
	.AddAuthenticationConfig();

var app = builder.Build();
app
	.UseEnvironment()
	.UseHttpsRedirection()
	.UseStaticFiles()
	.UseRouting()
	.UseCookiePolicyConfig()
    .UseAuthentication()
	.UseAuthorization()
	.UseEndPoints();

app.Run();