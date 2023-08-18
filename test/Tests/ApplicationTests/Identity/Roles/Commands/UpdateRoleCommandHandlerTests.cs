// ------------------------------------------------------------------------------------------------
//  <copyright file="UpdateRoleCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Roles.Commands;

using Application.Identity.Roles.Commands.UpdateRole;
using Application.Identity.Roles.Models;
using Domain.Errors;
using Domain.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Moq;

public class UpdateRoleCommandHandlerTests
{
    private readonly Mock<RoleManager<Role>> roleManager;

    public UpdateRoleCommandHandlerTests()
    {
        roleManager = new Mock<RoleManager<Role>>(
            Mock.Of<IRoleStore<Role>>(),
            null, null, null, null);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<UpdateRoleCommandHandler> act = () => new UpdateRoleCommandHandler(null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'roleManager')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenRoleDoesNotExists()
    {
        //Arrange
        this.roleManager.Setup(rm => rm.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<Role?>(null));
        var handler = new UpdateRoleCommandHandler(this.roleManager.Object);
        var request = new UpdateRoleRequest(Guid.NewGuid(), "New Name");
        var command = request.Adapt<UpdateRoleCommand>();

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Role.NonExistsRole, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenRoleNewNameExists()
    {
        //Arrange
        this.roleManager.Setup(rm => rm.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<Role?>(new Role()));
        this.roleManager.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(true));
        var handler = new UpdateRoleCommandHandler(this.roleManager.Object);
        var request = new UpdateRoleRequest(Guid.NewGuid(), "New Name");
        var command = request.Adapt<UpdateRoleCommand>();

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Role.RoleAlreadyExists, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenUpdateFailed()
    {
        //Arrange
        this.roleManager.Setup(rm => rm.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<Role?>(new Role()));
        this.roleManager.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(false));
        this.roleManager.Setup(rm => rm.UpdateAsync(It.IsAny<Role>()))
            .Returns(Task.FromResult<IdentityResult>(IdentityResult.Failed(new IdentityError[]
                { new() { Code = "Code", Description = "Description" } })));
        var handler = new UpdateRoleCommandHandler(this.roleManager.Object);
        var request = new UpdateRoleRequest(Guid.NewGuid(), "New Name");
        var command = request.Adapt<UpdateRoleCommand>();

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.AnUnexpectedError(nameof(UpdateRoleCommand)), result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        //Arrange
        this.roleManager.Setup(rm => rm.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<Role?>(new Role()));
        this.roleManager.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(false));
        this.roleManager.Setup(rm => rm.UpdateAsync(It.IsAny<Role>()))
            .Returns(Task.FromResult<IdentityResult>(IdentityResult.Success));
        var handler = new UpdateRoleCommandHandler(this.roleManager.Object);
        var request = new UpdateRoleRequest(Guid.NewGuid(), "New Name");
        var command = request.Adapt<UpdateRoleCommand>();

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }
}