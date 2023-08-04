namespace Infrastructure;

using Application.Abstractions;
using Application.Interfaces;
using Email;
using Infrastructure.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(
	this IServiceCollection services,
	IConfiguration configuration)
	{
		services.AddScoped<IJwtProvider, JwtProvider>();
		services.AddScoped<IEmailSender, MailService>();

		return services;
	}
}
