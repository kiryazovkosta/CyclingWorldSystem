namespace Persistence;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

public static class ApplyAutomaticaMigration
{
	public static WebApplication MigrateDatabase(this WebApplication app)
	{
		using var scope = app.Services.CreateScope();
		using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		try
		{
			context.Database.Migrate();

			var initializer = scope.ServiceProvider.GetService<ApplicationInitializer>();
			initializer?.SeedAsync().Wait();
		}
		catch (Exception ex)
		{
			throw;
		}

		return app;
	}
}