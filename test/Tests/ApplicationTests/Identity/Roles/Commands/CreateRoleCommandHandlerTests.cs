// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateRoleCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Roles.Commands;

using Application.Identity.Roles.Commands.CreateRole;
using Application.Identity.Roles.Models;
using Domain.Errors;
using Domain.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Moq;

public class CreateRoleCommandHandlerTests
{
    private readonly Mock<RoleManager<Role>> roleManager;

    public CreateRoleCommandHandlerTests()
    {
        roleManager = new Mock<RoleManager<Role>>(
            Mock.Of<IRoleStore<Role>>(),
            null, null, null, null);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<CreateRoleCommandHandler> act = () => new CreateRoleCommandHandler(null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'roleManager')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenRoleAlreadyExists()
    {
        //Arrange
        this.roleManager.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(true));
        var handler = new CreateRoleCommandHandler(this.roleManager.Object);
        var request = new CreateRoleRequest("RoleName");
        var command = request.Adapt<CreateRoleCommand>();

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Role.RoleAlreadyExists, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenCreateFailure()
    {
        //Arrange
        this.roleManager.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(false));
        this.roleManager.Setup(rm => rm.CreateAsync(It.IsAny<Role>()))
            .Returns(Task.FromResult<IdentityResult>(IdentityResult.Failed(new IdentityError[]
                { new() { Code = "Code", Description = "Description" } })));
        var handler = new CreateRoleCommandHandler(this.roleManager.Object);
        var request = new CreateRoleRequest("RoleName");
        var command = request.Adapt<CreateRoleCommand>();

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Role.FailedToCreate, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        //Arrange
        this.roleManager.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(false));
        this.roleManager.Setup(rm => rm.CreateAsync(It.IsAny<Role>()))
            .Returns(Task.FromResult<IdentityResult>(IdentityResult.Success));
        var handler = new CreateRoleCommandHandler(this.roleManager.Object);
        var request = new CreateRoleRequest("RoleName");
        var command = request.Adapt<CreateRoleCommand>();

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }

}