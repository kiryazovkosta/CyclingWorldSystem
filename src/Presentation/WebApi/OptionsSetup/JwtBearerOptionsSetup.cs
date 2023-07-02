﻿namespace WebApi.OptionsSetup;

using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class JwtBearerOptionsSetup : IConfigureOptions<JwtBearerOptions>
{
	private readonly JwtOptions _jwtOptions;

	public JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions)
	{
		_jwtOptions = jwtOptions.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
	}

	public void Configure(JwtBearerOptions options)
	{
		options.TokenValidationParameters = new()
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = _jwtOptions.Issuer,
			ValidAudience = _jwtOptions.Audience,
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(_jwtOptions.SecretKey))
		};
	}
}