// ------------------------------------------------------------------------------------------------
//  <copyright file="ResetPasswordCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Users.Commands;

using Application.Identity.Users.Commands.ResetPassword;
using Application.Identity.Users.Models;
using Domain.Errors;
using Domain.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Moq;

public class ResetPasswordCommandHandlerTests
{
    private readonly MockUserManager userManager = new();
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<ResetPasswordCommandHandler> act = () => new ResetPasswordCommandHandler(null!);

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
        var handler = new ResetPasswordCommandHandler(this.userManager.Object);
        var command = new ResetPasswordCommand("UserName", "Email@example.com", "FirstName", "LastName");
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.User.NonExistsUser, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenPasswordsAreNotEquals()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User()));
        var handler = new ResetPasswordCommandHandler(this.userManager.Object);
        var command = new ResetPasswordCommand("12345678", "a1s2d3f4", "P@ssw0rd", "P@ssw0rd1");
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.User.PasswordsAreNotEqual, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenResetPasswordIsNotSuccess()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User()));
        this.userManager.Setup(um => um.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            . Returns(Task.FromResult<IdentityResult>(IdentityResult.Failed(new IdentityError[]
                { new() { Code = "Code", Description = "Description" } })));
        var handler = new ResetPasswordCommandHandler(this.userManager.Object);
        var command = new ResetPasswordCommand("12345678", "a1s2d3f4", "P@ssw0rd", "P@ssw0rd");
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.AnUnexpectedError(nameof(ResetPasswordCommand)), result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User()));
        this.userManager.Setup(um => um.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            . Returns(Task.FromResult<IdentityResult>(IdentityResult.Success));
        var handler = new ResetPasswordCommandHandler(this.userManager.Object);
        var request = new ResetPasswordRequest("12345678", "a1s2d3f4", "P@ssw0rd", "P@ssw0rd");
        var command = request.Adapt<ResetPasswordCommand>();
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.True(result.Value);
    }
}