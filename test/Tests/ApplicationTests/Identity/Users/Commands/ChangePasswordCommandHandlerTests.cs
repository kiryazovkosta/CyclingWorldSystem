// ------------------------------------------------------------------------------------------------
//  <copyright file="ChangePasswordCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Users.Commands;

using Application.Identity.Users.Commands.ChangePassword;
using Application.Identity.Users.Models;
using Domain.Errors;
using Domain.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Moq;

public class ChangePasswordCommandHandlerTests
{
    private readonly MockUserManager userManager = new ();
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenUserManagerIsNull()
    {
        //Arrange & Act
        Func<ChangePasswordCommandHandler> act = () => new ChangePasswordCommandHandler(null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'userManager')", exception.Message);
    }
    
    [Fact]
    public async Task Handler_Should_ReturnFailureWhenUsedDoesNotExists()
    {
        //Arrange
        this.userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(null));
        var handler = new ChangePasswordCommandHandler(this.userManager.Object);
        var request = new ChangePasswordRequest("UserName", "OldPassword", "NewPassword", "NewPassword");
        var command = request.Adapt<ChangePasswordCommand>();
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.User.NonExistsUser, result.Error);
    }
    
    [Fact]
    public async Task Handler_Should_ReturnFailureWhenNewPasswordsDoesNotMatch()
    {
        //Arrange
        this.userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User()));
        var handler = new ChangePasswordCommandHandler(this.userManager.Object);
        var request = new ChangePasswordRequest("UserName", "OldPassword", "Newp", "Diff");
        var command = request.Adapt<ChangePasswordCommand>();
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.User.PasswordsAreNotEqual, result.Error);
    }
    
    [Fact]
    public async Task Handler_Should_ReturnFailureWhenChangePasswordFailed()
    {
        //Arrange
        this.userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User()));
        this.userManager.Setup(um => um.ChangePasswordAsync(
                It.IsAny<User>(), 
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .Returns(Task.FromResult<IdentityResult>(IdentityResult.Failed(
                new IdentityError[] { new() { Code = "Code", Description = "Description" } })));
        var handler = new ChangePasswordCommandHandler(this.userManager.Object);
        var request = new ChangePasswordRequest("UserName", "OldPassword", "P@ssw0rd", "P@ssw0rd");
        var command = request.Adapt<ChangePasswordCommand>();
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.User.FailedToSignIn, result.Error);
    }
    
    [Fact]
    public async Task Handler_Should_ReturnSuccess()
    {
        //Arrange
        this.userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User()));
        this.userManager.Setup(um => um.ChangePasswordAsync(
                It.IsAny<User>(), 
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .Returns(Task.FromResult<IdentityResult>(IdentityResult.Success));
        var handler = new ChangePasswordCommandHandler(this.userManager.Object);
        var request = new ChangePasswordRequest("UserName", "OldPassword", "P@ssw0rd", "P@ssw0rd");
        var command = request.Adapt<ChangePasswordCommand>();
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.True(result.Value);
    }
}