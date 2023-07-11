namespace Application;

using Application.Behaviors;
using Application.Interfaces;
using Application.Services;
using FluentValidation;
using MediatR;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddMediatR(config =>
		{
			config.RegisterServicesFromAssemblyContaining<AssemblyReference>();

			config.NotificationPublisher = new TaskWhenAllPublisher();
		});

		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

		services.AddValidatorsFromAssembly(Application.AssemblyReference.Assembly, includeInternalTypes: true);

		services.AddHttpContextAccessor();
		services.AddScoped<ICurrentUserService, CurrentUserService>();

		services.AddScoped<IGeoCoordinate, GeoCoordinate>();
		services.AddScoped<IGpxService, GpxService>();

		return services;
	}
}