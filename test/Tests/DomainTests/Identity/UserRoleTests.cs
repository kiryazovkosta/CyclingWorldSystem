using Domain.Identity;

namespace Tests.DomainTests.Identity;

public class UserRoleTests
{
    [Fact]
    public void UserRole_User_SetAndGet()
    {
        // Arrange
        var userRole = new UserRole();
        var user = new User();

        // Act
        userRole.User = user;

        // Assert
        Assert.Same(user, userRole.User);
    }

    [Fact]
    public void UserRole_Role_SetAndGet()
    {
        // Arrange
        var userRole = new UserRole();
        var role = new Role();

        // Act
        userRole.Role = role;

        // Assert
        Assert.Same(role, userRole.Role);
    }
}
