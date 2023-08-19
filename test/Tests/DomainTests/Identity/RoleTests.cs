using Domain.Identity;

namespace Tests.DomainTests.Identity;

public class RoleTests
{
    [Fact]
    public void Role_DefaultValues_SetCorrectly()
    {
        // Arrange
        var role = new Role();

        // Assert
        Assert.Equal(default(DateTime), role.CreatedOn);
        Assert.Null(role.ModifiedOn);
        Assert.False(role.IsDeleted);
        Assert.Null(role.DeletedOn);
    }

    [Fact]
    public void Role_CollectionsNotNull()
    {
        // Arrange
        var role = new Role();

        // Assert
        Assert.Empty(role.UserRoles);
    }
}
