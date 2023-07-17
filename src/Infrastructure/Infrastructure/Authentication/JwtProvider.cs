namespace Infrastructure.Authentication;

using Application.Abstractions;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public sealed class JwtProvider : IJwtProvider
{
	private readonly JwtOptions _jWtOptions;

	public JwtProvider(IOptions<JwtOptions> jwtOptions)
	{
		this._jWtOptions = jwtOptions.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
	}

	public string CreateToken(User user, IEnumerable<string> roles)
	{
		var expiration = DateTime.UtcNow.AddMinutes(this._jWtOptions.ExpirationMinutes);
		var token = CreateJwtToken(
			CreateClaims(user, roles),
			CreateSigningCredentials(),
			expiration
		);
		var tokenHandler = new JwtSecurityTokenHandler();
		return tokenHandler.WriteToken(token);
	}

	private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
		DateTime expiration) =>
		new(
			this._jWtOptions.Issuer,
			this._jWtOptions.Audience,
			claims,
			expires: expiration,
			signingCredentials: credentials
		);

	private List<Claim> CreateClaims(User user, IEnumerable<string> roles)
	{
		try
		{
			var claims = new List<Claim>
				{
					new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
					new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
					new Claim("identifier", user.Id.ToString()),
					new Claim("email", user.Email!),
					new Claim("username", user.UserName!),
				};
			
			foreach (var role in roles) 
			{
				claims.Add(new Claim("role", role));
            }

			return claims;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}
	private SigningCredentials CreateSigningCredentials()
	{
		return new SigningCredentials(
			new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(this._jWtOptions.SecretKey)
			),
			SecurityAlgorithms.HmacSha256
		);
	}
}