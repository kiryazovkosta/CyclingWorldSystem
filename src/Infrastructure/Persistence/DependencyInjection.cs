namespace Persistence;

using Domain.Abstractions;
using Domain.Identity;
using Domain.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;
using Persistence.Repositories.Abstractions;
using Microsoft.AspNetCore.Identity;
using Persistence.Interseptors;

public static class DependencyInjection
{
	public static IServiceCollection AddPersistence(
	this IServiceCollection services,
	IConfiguration configuration)
	{
		services.AddSingleton<UpdateAuditableEntitiesInterseptor>();

		services.AddDbContext<ApplicationDbContext>(
			(sp, optionsBuilder) =>
			{
				var outboxInterseptor = sp.GetService<UpdateAuditableEntitiesInterseptor>();

				optionsBuilder.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"),
					o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
				.AddInterceptors(outboxInterseptor!);
			});

		services.AddDefaultIdentity<User>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequiredLength = 6;
				options.User.RequireUniqueEmail = true;
				options.SignIn.RequireConfirmedAccount = true;
			})
			.AddRoles<Role>()
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders()
			.AddDefaultTokenProviders();

		services.AddScoped<IUnitOfWork, UnitOfWork>();

		services.AddScoped<IBikeRepository, BikeRepository>();

		return services;
	}
}