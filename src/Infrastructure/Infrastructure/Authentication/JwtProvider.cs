namespace Infrastructure.Authentication;

using Application.Abstractions;
using Domain.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public sealed class JwtProvider : IJwtProvider
{
	private readonly JwtOptions _jwtOptions;

	public JwtProvider(IOptions<JwtOptions> jwtOptions)
	{
		_jwtOptions = jwtOptions.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
	}

	public string Generate(User user)
	{
		var claims = new Claim[] 
		{ 
			new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new(JwtRegisteredClaimNames.Name, user?.UserName ?? string.Empty)
		};

		var signingCredentials = new SigningCredentials(
			new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
			SecurityAlgorithms.HmacSha256
		);

		var token = new JwtSecurityToken(
			_jwtOptions.Issuer,
			_jwtOptions.Audience,
			claims,
			null,
			DateTime.UtcNow.AddHours(1),
			signingCredentials
		);
		
		string tokenValue = new JwtSecurityTokenHandler()
			.WriteToken(token);

		return tokenValue;
	}
}