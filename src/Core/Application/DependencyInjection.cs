namespace Application;

using Application.Behaviors;
using Application.Entities.Bikes.Commands.CreateBike;
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
		services.AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true);

		return services;
	}
}