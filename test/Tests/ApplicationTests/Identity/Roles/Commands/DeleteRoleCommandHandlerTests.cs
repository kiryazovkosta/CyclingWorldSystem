// ------------------------------------------------------------------------------------------------
//  <copyright file="DeleteRoleCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Roles.Commands;

using Application.Identity.Roles.Commands.DeleteRole;
using Domain.Errors;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Moq;

public class DeleteRoleCommandHandlerTests
{
    private readonly MockUserManager userManager = new();
    private readonly Mock<RoleManager<Role>> roleManager;

    public DeleteRoleCommandHandlerTests()
    {
        roleManager = new Mock<RoleManager<Role>>(
            Mock.Of<IRoleStore<Role>>(),
            null, null, null, null);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenUserManagerIsNull()
    {
        //Arrange & Act
        Func<DeleteRoleCommandHandler> act = () => new DeleteRoleCommandHandler(null!, this.roleManager.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'userManager')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenRoleManagerIsNull()
    {
        //Arrange & Act
        Func<DeleteRoleCommandHandler> act = () => new DeleteRoleCommandHandler(this.userManager.Object, null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'roleManager')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenRoleAlreadyExists()
    {
        //Arrange
        this.roleManager.Setup(rm => rm.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<Role?>(null));
        var handler = new DeleteRoleCommandHandler(this.userManager.Object, this.roleManager.Object);
        var command = new DeleteRoleCommand(Guid.NewGuid());

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Role.NonExistsRole, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenRoleContainsUsers()
    {
        //Arrange
        this.roleManager.Setup(rm => rm.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<Role?>(new Role()));
        this.userManager.Setup(um => um.GetUsersInRoleAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<IList<User>>(new List<User>() { new User() }));
        var handler = new DeleteRoleCommandHandler(this.userManager.Object, this.roleManager.Object);
        var command = new DeleteRoleCommand(Guid.NewGuid());

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Role.NonEmptyRole, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenDeleteFailed()
    {
        //Arrange
        this.roleManager.Setup(rm => rm.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<Role?>(new Role()));
        this.userManager.Setup(um => um.GetUsersInRoleAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<IList<User>>(new List<User>()));
        this.roleManager.Setup(rm => rm.DeleteAsync(It.IsAny<Role>()))
            .Returns(Task.FromResult<IdentityResult>(IdentityResult.Failed(new IdentityError[]
                { new() { Code = "Code", Description = "Description" } })));
        var handler = new DeleteRoleCommandHandler(this.userManager.Object, this.roleManager.Object);
        var command = new DeleteRoleCommand(Guid.NewGuid());

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.DeleteOperationFailed(command.Id, nameof(DeleteRoleCommand)), result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        //Arrange
        this.roleManager.Setup(rm => rm.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<Role?>(new Role()));
        this.userManager.Setup(um => um.GetUsersInRoleAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<IList<User>>(new List<User>()));
        this.roleManager.Setup(rm => rm.DeleteAsync(It.IsAny<Role>()))
            .Returns(Task.FromResult<IdentityResult>(IdentityResult.Success));
        var handler = new DeleteRoleCommandHandler(this.userManager.Object, this.roleManager.Object);
        var command = new DeleteRoleCommand(Guid.NewGuid());

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }
}