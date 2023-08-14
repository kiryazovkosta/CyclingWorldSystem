// ------------------------------------------------------------------------------------------------
//  <copyright file="JwtProviderTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.InfrastructureTests.Infrastructure.Authentication;

using System.Globalization;
using System.Security.Claims;
using Domain.Identity;
using global::Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;

public class JwtProviderTests
{
    [Fact]
    public void CreateToken_ValidUserAndRoles_ReturnsToken()
    {
        // Arrange
        var jwtOptions = Options.Create(new JwtOptions
        {
            SecretKey = "your-secure-secret-key-should-be-longer",
            ExpirationMinutes = 60,
            Issuer = "your-issuer",
            Audience = "your-audience"
        });
        var jwtProvider = new JwtProvider(jwtOptions);
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            UserName = "testUser",
            FirstName = "Test",
            LastName = "User",
            ImageUrl = "avatar.jpg"
        };
        var roles = new List<string> { "Admin", "User" };

        // Act
        var token = jwtProvider.CreateToken(user, roles);

        // Assert
        Assert.NotNull(token);
    }
    
    [Fact]
    public void CreateJwtToken_ValidClaims_ReturnsJwtSecurityToken()
    {
        // Arrange
        var jwtOptions = Options.Create(new JwtOptions
        {
            SecretKey = "your-secure-secret-key-should-be-longer",
            ExpirationMinutes = 60,
            Issuer = "your-issuer",
            Audience = "your-audience"
        });
        var jwtProvider = new JwtProvider(jwtOptions);
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, "1"),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new Claim("identifier", "1"),
            new Claim("email", "test@example.com"),
            new Claim("username", "testUser"),
            new Claim("fullname", "Test User"),
            new Claim("avatar", "avatar.jpg"),
            new Claim("role", "Admin")
        };
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            UserName = "testUser",
            FirstName = "Test",
            LastName = "User",
            ImageUrl = "avatar.jpg"
        };
        var roles = new List<string> { "Admin" };
        
        // Act
        var token = jwtProvider.CreateToken(user, roles);

        // Assert
        Assert.NotNull(token);
    }
}