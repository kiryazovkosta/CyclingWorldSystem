

namespace Persistence;

using Domain.Abstractions;
using Domain.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;
using Persistence.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class DependencyInjection
{
	public static IServiceCollection AddPersistence(
	this IServiceCollection services,
	IConfiguration configuration)
	{
		services.AddDbContext<ApplicationDbContext>(options =>
			options
				.UseSqlServer(configuration.GetConnectionString("DatabaseConnection")));

		services.AddScoped<IUnitOfWork, UnitOfWork>();

		services.AddScoped<IBikeRepository, BikeRepository>();

		return services;
	}
}