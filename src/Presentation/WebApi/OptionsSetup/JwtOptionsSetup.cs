namespace WebApi.OptionsSetup;

using Infrastructure.Authentication;
using Microsoft.Extensions.Options;

public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
{
	private const string SectionName = "Jwt";
	private readonly IConfiguration _configuration;

	public JwtOptionsSetup(IConfiguration configuration)
	{
		_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
	}

	public void Configure(JwtOptions options)
	{
		_configuration.GetSection(SectionName).Bind(options);
	}
}
