namespace Web.Configurations;

using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Security;

public static class HttpClientConfiguration
{
	public static IServiceCollection AddHttpClientExtension(this IServiceCollection services, IConfiguration configuration)
	{
        services.AddHttpClient("webApi", client =>
        {
            client.BaseAddress = new Uri(configuration["Backend:BaseUrl"] ?? string.Empty);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }).ConfigurePrimaryHttpMessageHandler(() =>
        {
            var sslOptions = new SslClientAuthenticationOptions
            {
                RemoteCertificateValidationCallback = delegate { return true; },
            };

            return new SocketsHttpHandler()
            {
                SslOptions = sslOptions,
                PooledConnectionLifetime = TimeSpan.FromMinutes(5),
            };
        })
        .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        return services;
	}
}