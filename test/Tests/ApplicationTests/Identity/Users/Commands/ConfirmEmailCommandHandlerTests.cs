// ------------------------------------------------------------------------------------------------
//  <copyright file="ConfirmEmailCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Users.Commands;

using Application.Identity.Users.Commands.ConfirmEmail;
using Application.Identity.Users.Models;
using Domain.Errors;
using Domain.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Moq;

public class ConfirmEmailCommandHandlerTests
{
    private readonly MockUserManager userManager = new ();
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenUserManagerIsNull()
    {
        //Arrange & Act
        Func<ConfirmEmailCommandHandler> act = () => new ConfirmEmailCommandHandler(null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'userManager')", exception.Message);
    }
    
    [Fact]
    public async Task Handler_Should_ReturnFailureWhenUsedDoesNotExists()
    {
        //Arrange
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(null));
        var handler = new ConfirmEmailCommandHandler(this.userManager.Object);
        var request = new ConfirmEmailRequest("12345678", "1a2s3d4f");
        var command = request.Adapt<ConfirmEmailCommand>();
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.User.NonExistsUser, result.Error);
    }
    
    [Fact]
    public async Task Handler_Should_ReturnFailureWhenConfirmationFailed()
    {
        //Arrange
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User()));
        this.userManager.Setup(um => um.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(Task.FromResult<IdentityResult>(IdentityResult.Failed(
                new IdentityError[] { new() { Code = "Code", Description = "Description" } })));
        var handler = new ConfirmEmailCommandHandler(this.userManager.Object);
        var request = new ConfirmEmailRequest("12345678", "1a2s3d4f");
        var command = request.Adapt<ConfirmEmailCommand>();
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.AnUnexpectedError(nameof(ConfirmEmailCommand)), result.Error);
    }
    
    [Fact]
    public async Task Handler_Should_ReturnSuccess()
    {
        //Arrange
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User()));
        this.userManager.Setup(um => um.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(Task.FromResult<IdentityResult>(IdentityResult.Success));
        var handler = new ConfirmEmailCommandHandler(this.userManager.Object);
        var request = new ConfirmEmailRequest("12345678", "1a2s3d4f");
        var command = request.Adapt<ConfirmEmailCommand>();
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        Assert.False(result.IsFailure);
        Assert.True(result.IsSuccess);
        Assert.True(result.Value);
    }
    

}