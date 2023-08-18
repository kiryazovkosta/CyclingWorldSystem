// ------------------------------------------------------------------------------------------------
//  <copyright file="ForgotPasswordCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Users.Commands;

using Application.Identity.Users.Commands.ForgotPassword;
using Application.Interfaces;
using Domain.Errors;
using Domain.Identity;
using Moq;
using Persistence;

public class ForgotPasswordCommandHandlerTests : IDisposable
{
    private readonly ApplicationDbContext context;
    private readonly MockUserManager userManager;
    private readonly Mock<IEmailSender> emailSender;

    public ForgotPasswordCommandHandlerTests()
    {
        this.context = ApplicationDbContextTestFactory.Create();
        this.userManager = new MockUserManager();
        this.emailSender = new Mock<IEmailSender>();
    }
    
    public void Dispose()
    {
        this.context.Dispose();
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<ForgotPasswordCommandHandler> act = () => new ForgotPasswordCommandHandler(
            null!,
            this.emailSender.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'userManager')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenEmailServiceIsNull()
    {
        //Arrange & Act
        Func<ForgotPasswordCommandHandler> act = () => new ForgotPasswordCommandHandler(
            this.userManager.Object,
            null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'emailSender')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenUserDoesNotExists()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(null));
        var handler = new ForgotPasswordCommandHandler(
            this.userManager.Object,
            this.emailSender.Object);
        var command = new ForgotPasswordCommand("UserName");
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.User.NonExistsUser, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenUserEmailIsNotConfirmed()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User()));
        this.userManager.Setup(um => um.IsEmailConfirmedAsync(It.IsAny<User>()))
            .Returns(Task.FromResult<bool>(false));
        var handler = new ForgotPasswordCommandHandler(
            this.userManager.Object,
            this.emailSender.Object);
        var command = new ForgotPasswordCommand("UserName");
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.User.NonExistsUser, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User()));
        this.userManager.Setup(um => um.IsEmailConfirmedAsync(It.IsAny<User>()))
            .Returns(Task.FromResult<bool>(true));
        this.userManager.Setup(um => um.GetUserIdAsync(It.IsAny<User>()))
            .Returns(Task.FromResult<string>("12345678"));
        this.userManager.Setup(um => um.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
            .Returns(Task.FromResult<string>("1a2s3d4f"));
        var handler = new ForgotPasswordCommandHandler(
            this.userManager.Object,
            this.emailSender.Object);
        var command = new ForgotPasswordCommand("UserName");
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.True(result.Value);
    }
    
}