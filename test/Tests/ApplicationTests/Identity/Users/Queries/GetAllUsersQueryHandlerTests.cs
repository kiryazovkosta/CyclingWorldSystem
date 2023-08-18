// ------------------------------------------------------------------------------------------------
//  <copyright file="GetAllUsersQueryHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Users.Queries;

using Application.Identity.Users.Queries.GetAllUsers;
using Domain;
using Domain.Identity;
using Domain.Identity.Dtos;
using Domain.Primitives;
using Domain.Repositories;
using Moq;
using Persistence.Infrastructure;
using Persistence.Repositories;

public class GetAllUsersQueryHandlerTests : TestApplicationContext
{
    private readonly MockUserManager userManager = new();
    private readonly Mock<IUserRepository> userRepository;

    public GetAllUsersQueryHandlerTests()
    {
        this.userRepository = new Mock<IUserRepository>();
    }

    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<GetAllUsersQueryHandler> act = () => new GetAllUsersQueryHandler(null!);
    
        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'userRepository')", exception.Message);
    }
}