// ------------------------------------------------------------------------------------------------
//  <copyright file="GetUserByIdQueryHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Users.Queries;

using Application.Identity.Users.Queries.GetUserById;
using Domain.Errors;
using Domain.Identity;
using Moq;

public class GetUserByIdQueryHandlerTests
{
    private readonly MockUserManager userManager = new();
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<GetUserByIdQueryHandler> act = () => new GetUserByIdQueryHandler(null!);
    
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
        var handler = new GetUserByIdQueryHandler(this.userManager.Object);
        var command = new GetUserByIdQuery(Guid.NewGuid());
        
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
        // Arrange
        var userId = Guid.NewGuid();
        var role = new Role() { Name = "UserRole" };
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User() { Id = userId, UserName = "User", Email = "user@test.com", 
                EmailConfirmed = true, FirstName = "User", LastName = "Name", ImageUrl = "Image", 
                UserRoles = new List<UserRole>() { new UserRole() { Role = role, UserId = userId} } }));
        var handler = new GetUserByIdQueryHandler(this.userManager.Object);
        var command = new GetUserByIdQuery(Guid.NewGuid());
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        var response = result.Value;
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(userId, response.Id);
        Assert.Equal("User", response.UserName);
        Assert.Equal("user@test.com", response.Email);
        Assert.True(response.EmailConfirmed);
        Assert.Equal("User", response.FirstName);
        Assert.Equal("Name", response.LastName);
        Assert.Equal("Image", response.ImageUrl);
    }
}