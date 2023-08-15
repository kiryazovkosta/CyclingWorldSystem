// ------------------------------------------------------------------------------------------------
//  <copyright file="UpdateUserRolesCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Users.Commands;

using Application.Identity.Users.Commands.UpdateUserRoles;
using Domain.Errors;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Moq;

public class UpdateUserRolesCommandHandlerTests
{
    private readonly MockUserManager userManager = new();
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<UpdateUserRolesCommandHandler> act = () => new UpdateUserRolesCommandHandler(null!);

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
        var handler = new UpdateUserRolesCommandHandler(this.userManager.Object);
        var command = new UpdateUserRolesCommand(Guid.NewGuid(),new string[] { "User", "Master" });
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.User.NonExistsUser, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenFailedToRemoveUsersRoles()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User()));
        this.userManager.Setup(um => um.GetRolesAsync(It.IsAny<User>()))
            .Returns(Task.FromResult<IList<string>>(new List<string>() { "one", "two" }));
        this.userManager.Setup(um => um.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
            . Returns(Task.FromResult<IdentityResult>(IdentityResult.Failed(new IdentityError[]
                { new() { Code = "Code", Description = "Description" } })));
        var handler = new UpdateUserRolesCommandHandler(this.userManager.Object);
        var command = new UpdateUserRolesCommand(Guid.NewGuid(),new string[] { "User", "Master" });
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.User.FailedToRemoveUserRoles, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenFailedToAssignRoles()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User()));
        this.userManager.Setup(um => um.GetRolesAsync(It.IsAny<User>()))
            .Returns(Task.FromResult<IList<string>>(new List<string>() { "one", "two" }));
        this.userManager.Setup(um => um.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
            . Returns(Task.FromResult<IdentityResult>(IdentityResult.Success));
        this.userManager.Setup(um => um.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
            . Returns(Task.FromResult<IdentityResult>(IdentityResult.Failed(new IdentityError[]
                { new() { Code = "Code", Description = "Description" } })));
        var handler = new UpdateUserRolesCommandHandler(this.userManager.Object);
        var command = new UpdateUserRolesCommand(Guid.NewGuid(),new string[] { "User", "Master" });
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.User.FailedToAssignUserRoles, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User()));
        this.userManager.Setup(um => um.GetRolesAsync(It.IsAny<User>()))
            .Returns(Task.FromResult<IList<string>>(new List<string>() { "one", "two" }));
        this.userManager.Setup(um => um.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
            . Returns(Task.FromResult<IdentityResult>(IdentityResult.Success));
        this.userManager.Setup(um => um.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
            . Returns(Task.FromResult<IdentityResult>(IdentityResult.Success));
        var handler = new UpdateUserRolesCommandHandler(this.userManager.Object);
        var command = new UpdateUserRolesCommand(Guid.NewGuid(),new string[] { "User", "Master" });
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }
}