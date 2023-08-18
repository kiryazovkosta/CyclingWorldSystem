// ------------------------------------------------------------------------------------------------
//  <copyright file="LoginUserCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Users.Commands;

using Application.Abstractions;
using Application.Identity.Users.Commands.LogInUser;
using Domain.Errors;
using Domain.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using Persistence;

public class LoginUserCommandHandlerTests : IDisposable
{
    private readonly ApplicationDbContext context;
    private readonly Mock<SignInManager<User>> signInManager;
    private readonly MockUserManager userManager;
    private readonly Mock<IJwtProvider> jwtProvider;

    public LoginUserCommandHandlerTests()
    {
        this.context = ApplicationDbContextTestFactory.Create();
        this.userManager = new MockUserManager();
        this.signInManager = new Mock<SignInManager<User>>(
            this.userManager.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<User>>(),
            null,
            null,
            null,
            null);
        this.jwtProvider = new Mock<IJwtProvider>();
    }
    
    public void Dispose()
    {
        this.context.Dispose();
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenSignInManagerIsNull()
    {
        //Arrange & Act
        Func<LoginUserCommandHandler> act = () => new LoginUserCommandHandler(
            null!,
            this.userManager.Object,
            this.jwtProvider.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'signInManager')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenUserManagerIsNull()
    {
        //Arrange & Act
        Func<LoginUserCommandHandler> act = () => new LoginUserCommandHandler(
            this.signInManager.Object,
            null!,
            this.jwtProvider.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'userManager')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenJwtProviderIsNull()
    {
        //Arrange & Act
        Func<LoginUserCommandHandler> act = () => new LoginUserCommandHandler(
            this.signInManager.Object,
            this.userManager.Object,
            null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'jwtProvider')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenUserDoesNotExists()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(null));
        var handler = new LoginUserCommandHandler(
            this.signInManager.Object,
            this.userManager.Object,
            this.jwtProvider.Object);
        var command = new LogInUserCommand("UserName","P@ssw0rd");
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.User.LogInFailed, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenUPasswordSignInIsInvalidEmailIsNotConfirmed()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User() { IsDeleted = false }));
        this.signInManager.Setup(um =>
                um.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .Returns(Task.FromResult(SignInResult.Failed));
        this.userManager.Setup(um => um.IsEmailConfirmedAsync(It.IsAny<User>()))
            .Returns(Task.FromResult(false));
        var handler = new LoginUserCommandHandler(
            this.signInManager.Object,
            this.userManager.Object,
            this.jwtProvider.Object);
        var command = new LogInUserCommand("UserName","P@ssw0rd");
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.User.EmailIsNotConfirmed, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenUPasswordSignInIsInvalidEmailIsConfirmed()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User() { IsDeleted = false }));
        this.signInManager.Setup(um =>
                um.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .Returns(Task.FromResult(SignInResult.Failed));
        this.userManager.Setup(um => um.IsEmailConfirmedAsync(It.IsAny<User>()))
            .Returns(Task.FromResult(true));
        var handler = new LoginUserCommandHandler(
            this.signInManager.Object,
            this.userManager.Object,
            this.jwtProvider.Object);
        var command = new LogInUserCommand("UserName","P@ssw0rd");
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.AnUnexpectedError(nameof(LogInUserCommand)), result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User() { IsDeleted = false, UserName = "TestName", Email = "Test@example.com" }));
        this.signInManager.Setup(um =>
                um.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .Returns(Task.FromResult(SignInResult.Success));
        this.userManager.Setup(um => um.IsEmailConfirmedAsync(It.IsAny<User>()))
            .Returns(Task.FromResult(true));
        this.userManager.Setup(um => um.GetRolesAsync(It.IsAny<User>()))
            .Returns(Task.FromResult<IList<string>>(new string[] { "User", "Manager" }));
        this.jwtProvider.Setup(um => um.CreateToken(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
            .Returns("jwt-token-string");
        var handler = new LoginUserCommandHandler(
            this.signInManager.Object,
            this.userManager.Object,
            this.jwtProvider.Object);
        var command = new LogInUserCommand("UserName","P@ssw0rd");
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        var response = result.Value;
        Assert.Equal("TestName", response.UserName);
        Assert.Equal("Test@example.com", response.Email);
        Assert.Equal("jwt-token-string", response.Token);
    }
    
}