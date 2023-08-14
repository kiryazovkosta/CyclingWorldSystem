// ------------------------------------------------------------------------------------------------
//  <copyright file="CurrentUserServiceTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Services;

using System.Security.Claims;
using Application.Services;
using Domain.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

public class CurrentUserServiceTests
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly CurrentUserService _currentUserService;

        public CurrentUserServiceTests()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(),
                null, null, null, null, null, null, null, null);
            _currentUserService = new CurrentUserService(
                _httpContextAccessorMock.Object,
                _userManagerMock.Object);
        }

        [Fact]
        public void GetCurrentUserId_ValidClaims_ReturnsUserId()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            _httpContextAccessorMock.Setup(mock => mock.HttpContext)
                .Returns(httpContext);

            // Act
            var result = _currentUserService.GetCurrentUserId();

            // Assert
            Assert.Equal(userId, result);
        }

        [Fact]
        public void GetCurrentUserId_InvalidClaims_ReturnsEmptyGuid()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(mock => mock.HttpContext)
                .Returns(httpContext);

            // Act
            var result = _currentUserService.GetCurrentUserId();

            // Assert
            Assert.Equal(Guid.Empty, result);
        }

        [Fact]
        public void GetCurrentUserId_NullHttpContext_ReturnsEmptyGuid()
        {
            // Arrange
            _httpContextAccessorMock.Setup(mock => mock.HttpContext)
                .Returns((HttpContext?)null!);

            // Act
            var result = _currentUserService.GetCurrentUserId();

            // Assert
            Assert.Equal(Guid.Empty, result);
        }
        
        [Fact]
        public void GetCurrentUser_ValidClaims_ReturnsUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            _httpContextAccessorMock.Setup(mock => mock.HttpContext)
                .Returns(httpContext);
            _userManagerMock.Setup(mock => mock.Users)
                .Returns(new List<User> { user }.AsQueryable());

            // Act
            var result = _currentUserService.GetCurrentUser();

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public void GetCurrentUser_InvalidClaims_ReturnsNull()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(mock => mock.HttpContext)
                .Returns(httpContext);

            // Act
            var result = _currentUserService.GetCurrentUser();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetCurrentUser_NullHttpContext_ReturnsNull()
        {
            // Arrange
            _httpContextAccessorMock.Setup(mock => mock.HttpContext)
                .Returns((HttpContext)null!);

            // Act
            var result = _currentUserService.GetCurrentUser();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetCurrentUser_UserNotFound_ReturnsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            _httpContextAccessorMock.Setup(mock => mock.HttpContext)
                .Returns(httpContext);
            _userManagerMock.Setup(mock => mock.Users)
                .Returns(new List<User>().AsQueryable());

            // Act
            var result = _currentUserService.GetCurrentUser();

            // Assert
            Assert.Null(result);
        }
}