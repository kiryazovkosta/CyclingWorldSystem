// ------------------------------------------------------------------------------------------------
//  <copyright file="GetUserRolesCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Users.Queries;

using Application.Identity.Users.Queries.GetUserRoles;
using Domain.Errors;
using Domain.Identity;
using Moq;

public class GetUserRolesCommandHandlerTests
{
    private readonly MockUserManager userManager = new();
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<GetUserRolesCommandHandler> act = () => new GetUserRolesCommandHandler(null!);
    
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
        var handler = new GetUserRolesCommandHandler(this.userManager.Object);
        var command = new GetUserRolesCommand(Guid.NewGuid());
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.User.NonExistsUser, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        var userId = Guid.NewGuid();
        
        // Arrange
        var role = new Role() { Name = "Manager" };
        var users = new User[]
        {
            new User()
            {
                Id = userId, UserName = "User1", Email = "user1@example.com", FirstName = "User1", LastName = "Name1",
                UserRoles = new List<UserRole>()
                {
                    new UserRole() { Role = role, UserId = userId }
                }
            },
            new User()
            {
                Id = Guid.NewGuid(), UserName = "User2", Email = "user2@example.com", FirstName = "User2", LastName = "Name3"
            },
            new User()
            {
                Id = Guid.NewGuid(), UserName = "User3", Email = "user3@example.com", FirstName = "User3", LastName = "Name3"
            }
        }.AsQueryable();
        
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User() { Id = userId }));
        this.userManager.Setup(um => um.Users).Returns(users);
        var handler = new GetUserRolesCommandHandler(this.userManager.Object);
        var command = new GetUserRolesCommand(Guid.NewGuid());
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.False(result.IsFailure);
        Assert.True(result.IsSuccess);
        var roles = result.Value.ToList();
        Assert.Single(roles);
        Assert.Equal("Manager", roles.First());
        
    }
}