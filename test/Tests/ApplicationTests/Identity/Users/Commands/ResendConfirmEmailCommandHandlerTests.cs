// ------------------------------------------------------------------------------------------------
//  <copyright file="ResendConfirmEmailCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Users.Commands;

using Application.Identity.Users.Commands.ResendConfirmEmail;
using Application.Identity.Users.Models;
using Application.Interfaces;
using Domain.Errors;
using Domain.Identity;
using Domain.Repositories.Abstractions;
using Mapster;
using Moq;
using Persistence;

public class ResendConfirmEmailCommandHandlerTests: IDisposable
{
    private readonly ApplicationDbContext context = ApplicationDbContextTestFactory.Create();
    private readonly MockUserManager userManager = new();
    private readonly Mock<IEmailSender> emailSender = new();
    private readonly Mock<IUnitOfWork> unitOfWork = new();

    public void Dispose()
    {
        this.context.Dispose();
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<ResendConfirmEmailCommandHandler> act = () => new ResendConfirmEmailCommandHandler(
            null!,
            this.emailSender.Object,
            this.unitOfWork.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'userManager')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenIActivityLikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<ResendConfirmEmailCommandHandler> act = () => new ResendConfirmEmailCommandHandler(
            this.userManager.Object,
            null!,
            this.unitOfWork.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'emailSender')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenUserManagerIsNull()
    {
        //Arrange & Act
        Func<ResendConfirmEmailCommandHandler> act = () => new ResendConfirmEmailCommandHandler(
            this.userManager.Object,
            this.emailSender.Object,
            null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'context')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenUserDoesNotExists()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(null));
        var handler = new ResendConfirmEmailCommandHandler(this.userManager.Object, this.emailSender.Object, this.unitOfWork.Object);
        var request = new ResendConfirmEmailRequest("Email@example.com");
        var command = request.Adapt<ResendConfirmEmailCommand>();
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.User.NonExistsUser, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenEmailIsConfirmed()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User(){ EmailConfirmed = true }));
        var handler = new ResendConfirmEmailCommandHandler(this.userManager.Object, this.emailSender.Object, this.unitOfWork.Object);
        var request = new ResendConfirmEmailRequest("Email@example.com");
        var command = request.Adapt<ResendConfirmEmailCommand>();
        
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
        this.userManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User(){ EmailConfirmed = false }));
        this.userManager.Setup(um => um.GetUserIdAsync(It.IsAny<User>()))
            .Returns(Task.FromResult<string>("12345678"));
        this.userManager.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
            .Returns(Task.FromResult<string>("1sd3f4"));
        var handler = new ResendConfirmEmailCommandHandler(this.userManager.Object, this.emailSender.Object, this.unitOfWork.Object);
        var request = new ResendConfirmEmailRequest("Email@example.com");
        var command = request.Adapt<ResendConfirmEmailCommand>();
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.True(result.Value);
    }
}