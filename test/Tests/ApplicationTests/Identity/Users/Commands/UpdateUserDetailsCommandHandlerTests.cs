// ------------------------------------------------------------------------------------------------
//  <copyright file="UpdateUserDetailsCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Users.Commands;

using Application.Identity.Users.Commands.UpdateUserDetails;
using Domain.Errors;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Moq;

public class UpdateUserDetailsCommandHandlerTests
{
    private readonly MockUserManager userManager = new();
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<UpdateUserDetailsCommandHandler> act = () => new UpdateUserDetailsCommandHandler(null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'userManager')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenUserDoesNotExists()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(null));
        var handler = new UpdateUserDetailsCommandHandler(this.userManager.Object);
        var command = new UpdateUserDetailsCommand("UserName", "Email@example.com", "FirstName", "LastName");
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.User.NonExistsUser, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenUpdateFailed()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User()));
        this.userManager.Setup(um => um.UpdateAsync(It.IsAny<User>()))
            . Returns(Task.FromResult<IdentityResult>(IdentityResult.Failed(new IdentityError[]
                { new() { Code = "Code", Description = "Description" } })));
        var handler = new UpdateUserDetailsCommandHandler(this.userManager.Object);
        var command = new UpdateUserDetailsCommand("UserName", "Email@example.com", "FirstName", "LastName");
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.User.FailedToUpdateUser, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User()));
        this.userManager.Setup(um => um.UpdateAsync(It.IsAny<User>()))
            . Returns(Task.FromResult<IdentityResult>(IdentityResult.Success));
        var handler = new UpdateUserDetailsCommandHandler(this.userManager.Object);
        var command = new UpdateUserDetailsCommand("UserName", "Email@example.com", "FirstName", "LastName");
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.True(result.Value);
    }
}