// ------------------------------------------------------------------------------------------------
//  <copyright file="GetRoleByIdQueryHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Roles.Queries;

using Application.Identity.Roles.Queries.GetRoleById;
using Domain.Errors;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Moq;

public class GetRoleByIdQueryHandlerTests
{
    private readonly Mock<RoleManager<Role>> roleManager;

    public GetRoleByIdQueryHandlerTests()
    {
        roleManager = new Mock<RoleManager<Role>>(
            Mock.Of<IRoleStore<Role>>(),
            null, null, null, null);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<GetRoleByIdQueryHandler> act = () => new GetRoleByIdQueryHandler(null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'roleManager')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenRoleDoesNotExists()
    {
        //Arrange
        roleManager.Setup(rm => rm.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<Role?>(null));
        var handler = new GetRoleByIdQueryHandler(this.roleManager.Object);
        var command = new GetRoleByIdQuery(TestsContants.UserRoleId);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.Role.NonExistsRole, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccessWhenRoleDoesNotExists()
    {
        //Arrange
        roleManager.Setup(rm => rm.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<Role?>(new Role() { Name = "RoleName"}));
        var handler = new GetRoleByIdQueryHandler(this.roleManager.Object);
        var command = new GetRoleByIdQuery(TestsContants.UserRoleId);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        var role = result.Value;
        Assert.Equal("RoleName", role.Name);
    }
}