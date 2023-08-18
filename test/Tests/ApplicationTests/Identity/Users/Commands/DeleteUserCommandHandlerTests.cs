// ------------------------------------------------------------------------------------------------
//  <copyright file="DeleteUserCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Users.Commands;

using Application.Identity.Users.Commands.DeleteUser;
using Application.Interfaces;
using Domain.Errors;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Moq;
using Persistence;

public class DeleteUserCommandHandlerTests
{
    private readonly MockUserManager userManager;
    private readonly Mock<ICurrentUserService> currentUserService;

    public DeleteUserCommandHandlerTests()
    {
        this.userManager = new MockUserManager();
        this.currentUserService = new Mock<ICurrentUserService>();
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<DeleteUserCommandHandler> act = () => new DeleteUserCommandHandler(
            null!,
            this.currentUserService.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'userManager')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenEmailServiceIsNull()
    {
        //Arrange & Act
        Func<DeleteUserCommandHandler> act = () => new DeleteUserCommandHandler(
            this.userManager.Object,
            null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'currentUserService')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenUserDoesNotExists()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(null));
        var handler = new DeleteUserCommandHandler(
            this.userManager.Object,
            this.currentUserService.Object);
        var command = new DeleteUserCommand(Guid.NewGuid());
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.DeleteOperationFailed(command.Id, nameof(DeleteUserCommand)), result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenUserTryToDeleteHimself()
    {
        // Arrange
        var userId = Guid.NewGuid();
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User() { Id = userId }));
        this.currentUserService.Setup(um => um.GetCurrentUserId())
            .Returns(userId);
        var handler = new DeleteUserCommandHandler(
            this.userManager.Object,
            this.currentUserService.Object);
        var command = new DeleteUserCommand(userId);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.AnUnexpectedError(nameof(DeleteUserCommand)), result.Error);
    } 
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenDeleteFailed()
    {
        // Arrange
        var userId = Guid.NewGuid();
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User() { Id = userId }));
        this.currentUserService.Setup(um => um.GetCurrentUserId())
            .Returns(Guid.NewGuid());
        this.userManager.Setup(um => um.DeleteAsync(It.IsAny<User>()))
            .Returns(Task.FromResult<IdentityResult>(IdentityResult.Failed(
                new IdentityError[] { new() { Code = "Code", Description = "Description" } })));
        var handler = new DeleteUserCommandHandler(
            this.userManager.Object,
            this.currentUserService.Object);
        var command = new DeleteUserCommand(userId);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.AnUnexpectedError(nameof(DeleteUserCommand)), result.Error);
    } 
    
    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User() { Id = userId }));
        this.currentUserService.Setup(um => um.GetCurrentUserId())
            .Returns(Guid.NewGuid());
        this.userManager.Setup(um => um.DeleteAsync(It.IsAny<User>()))
            .Returns(Task.FromResult<IdentityResult>(IdentityResult.Success));
        var handler = new DeleteUserCommandHandler(
            this.userManager.Object,
            this.currentUserService.Object);
        var command = new DeleteUserCommand(userId);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    } 
}