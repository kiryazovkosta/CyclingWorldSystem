// ------------------------------------------------------------------------------------------------
//  <copyright file="GetAllRolesQueryHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Roles.Queries;

using Application.Identity.Roles.Queries.GetAllRoles;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Moq;

public class GetAllRolesQueryHandlerTests
{
    private readonly Mock<RoleManager<Role>> roleManager;

    public GetAllRolesQueryHandlerTests()
    {
        roleManager = new Mock<RoleManager<Role>>(
            Mock.Of<IRoleStore<Role>>(),
            null, null, null, null);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<GetAllRolesQueryHandler> act = () => new GetAllRolesQueryHandler(null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'roleManager')", exception.Message);
    }
}