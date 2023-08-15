// ------------------------------------------------------------------------------------------------
//  <copyright file="UpdateUserCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Users.Commands;

using Application.Identity.Users.Commands.DeleteUser;
using Application.Identity.Users.Commands.UpdateUser;
using Application.Identity.Users.Models;
using Domain.Errors;
using Domain.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Moq;

public class UpdateUserCommandHandlerTests
{
    private readonly MockUserManager userManager = new();
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<UpdateUserCommandHandler> act = () => new UpdateUserCommandHandler(null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'userManager')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenUserDoesNotExists()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(null));
        var handler = new UpdateUserCommandHandler(this.userManager.Object);
        var command = new UpdateUserCommand(Guid.NewGuid(),"UserName", "Email@example.com", true, "FirstName",
            "FirstName", "P@ssw0rd");
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.DeleteOperationFailed(command.Id, nameof(DeleteUserCommand)), result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenUpdateFailed()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User()));
        this.userManager.Setup(um => um.UpdateAsync(It.IsAny<User>()))
            . Returns(Task.FromResult<IdentityResult>(IdentityResult.Failed(new IdentityError[]
                { new() { Code = "Code", Description = "Description" } })));
        var handler = new UpdateUserCommandHandler(this.userManager.Object);
        var command = new UpdateUserCommand(Guid.NewGuid(),"UserName", "Email@example.com", true, "FirstName",
            "FirstName", "P@ssw0rd");
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.User.FailedToUpdateUser, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenUpdateOfPasswordFailed()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User()));
        this.userManager.Setup(um => um.UpdateAsync(It.IsAny<User>()))
            . Returns(Task.FromResult<IdentityResult>(IdentityResult.Success));
        this.userManager.Setup(um => um.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
            .Returns(Task.FromResult<string>("a1s2d3f4"));
        this.userManager.Setup(um => um.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult<IdentityResult>(IdentityResult.Failed(new IdentityError[]
                { new() { Code = "Code", Description = "Description" } })));
        var handler = new UpdateUserCommandHandler(this.userManager.Object);
        var command = new UpdateUserCommand(Guid.NewGuid(),"UserName", "Email@example.com", true, "FirstName",
            "FirstName", "P@ssw0rd");
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.User.FailedToUpdatePassword, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccessWhenUpdate()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User()));
        this.userManager.Setup(um => um.UpdateAsync(It.IsAny<User>()))
            . Returns(Task.FromResult<IdentityResult>(IdentityResult.Success));
        this.userManager.Setup(um => um.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
            .Returns(Task.FromResult<string>("a1s2d3f4"));
        this.userManager.Setup(um => um.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult<IdentityResult>(IdentityResult.Success));
        var handler = new UpdateUserCommandHandler(this.userManager.Object);
        var request = new UpdateUserRequest(Guid.NewGuid(),"UserName", "Email@example.com", true, "FirstName",
            "FirstName", "P@ssw0rd");
        var command = request.Adapt<UpdateUserCommand>();
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }
}