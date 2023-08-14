using Common.Constants;
using Domain.Identity;

namespace Tests.DomainTests.Identity;

public class UserTests
{
    [Fact]
    public void User_FullName_CorrectFormat()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe"
        };

        // Act
        var fullName = user.FullName;

        // Assert
        Assert.Equal("John Doe", fullName);
    }

    [Fact]
    public void User_Update_CorrectValuesSet()
    {
        // Arrange
        var user = new User();
        var newUserName = "newuser";
        var newEmail = "newemail@example.com";
        var newIsConfirmed = true;
        var newFirstName = "New";
        var newLastName = "User";

        // Act
        var result = user.Update(newUserName, newEmail, newIsConfirmed, newFirstName, newLastName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newUserName, user.UserName);
        Assert.Equal(newEmail, user.Email);
        Assert.Equal(newIsConfirmed, user.EmailConfirmed);
        Assert.Equal(newFirstName, user.FirstName);
        Assert.Equal(newLastName, user.LastName);
    }

    [Fact]
    public void User_CreatedOn_DefaultValue()
    {
        // Arrange
        var user = new User();

        // Assert
        Assert.Equal(default(DateTime), user.CreatedOn);
    }

    [Fact]
    public void User_ModifiedOn_NullByDefault()
    {
        // Arrange
        var user = new User();

        // Assert
        Assert.Null(user.ModifiedOn);
    }

    [Fact]
    public void User_IsDeleted_FalseByDefault()
    {
        // Arrange
        var user = new User();

        // Assert
        Assert.False(user.IsDeleted);
    }

    [Fact]
    public void User_DeletedOn_NullByDefault()
    {
        // Arrange
        var user = new User();

        // Assert
        Assert.Null(user.DeletedOn);
    }

    [Fact]
    public void User_DefaultImageUrl_SetCorrectly()
    {
        // Arrange
        var user = new User();

        // Assert
        Assert.Equal(GlobalConstants.Cloudinary.DefaultAvatar, user.ImageUrl);
    }

    [Fact]
    public void User_CollectionsAreEmpty()
    {
        // Arrange
        var user = new User();

        // Assert
        Assert.Empty(user.Activities);
        Assert.Empty(user.Comments);
        Assert.Empty(user.Likes);
        Assert.Empty(user.Challenges);
        Assert.Empty(user.TrainingPlans);
    }
}
