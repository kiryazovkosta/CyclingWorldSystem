namespace WebApi.OptionsSetup;

using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class JwtBearerOptionsSetup : IPostConfigureOptions<JwtBearerOptions>
{
	private readonly JwtOptions _jwtOptions;

	public JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions)
	{
		_jwtOptions = jwtOptions.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
	}

	public void PostConfigure(string? name, JwtBearerOptions options)
	{
		options.TokenValidationParameters.ValidIssuer = this._jwtOptions.Issuer;
		options.TokenValidationParameters.ValidAudience = this._jwtOptions.Audience;
		options.TokenValidationParameters.IssuerSigningKey =
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._jwtOptions.SecretKey));
	}
}