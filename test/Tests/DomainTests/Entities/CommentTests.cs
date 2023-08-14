using Common.Constants;
using Domain.Entities;
using Domain.Errors;

namespace Tests.DomainTests.Entities;

public class CommentTests
{
    [Fact]
    public void Comment_Create_ValidData_ReturnsComment()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var content = "Test content";

        // Act
        var result = Comment.Create(activityId, userId, content);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(activityId, result.Value.ActivityId);
        Assert.Equal(userId, result.Value.UserId);
        Assert.Equal(content, result.Value.Content);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Comment_Create_EmptyOrNullOrWhiteSpaceContent_Failure(string content)
    {
        // Arrange
        var activityId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        // Act
        var result = Comment.Create(activityId, userId, content);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Comments.ContentIsEmptyOrNull, result.Error);
    }

    [Theory]
    [InlineData("aaaa")]
    [InlineData("aaaaaaaaa1aaaaaaaaa2aaaaaaaaa3aaaaaaaaa4aaaaaaaaa5aaaaaaaaa1aaaaaaaaa2aaaaaaaaa3aaaaaaaaa4aaaaaaaaa5aaaaaaaaa1aaaaaaaaa2aaaaaaaaa3aaaaaaaaa4aaaaaaaaa5aaaaaaaaa1aaaaaaaaa2aaaaaaaaa3aaaaaaaaa4aaaaaaaaa5aaaaaaaaa1aaaaaaaaa2aaaaaaaaa3aaaaaaaaa4aaaaaaaaa5aaaaaaaaa1aaaaaaaaa2aaaaaaaaa3aaaaaaaaa4aaaaaaaaa5")]
    public void Comment_Create_InvalidContentLength_Failure(string content)
    {
        // Arrange
        var activityId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        // Act
        var result = Comment.Create(activityId, userId, content);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Comments.ContentInvalidLength, result.Error);
    }

    [Fact]
    public void Comment_Update_ContentUpdated()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var comment = Comment.Create(activityId, userId, "Initial content").Value;
        var newContent = "Updated content";

        // Act
        comment.Update(newContent);

        // Assert
        Assert.Equal(newContent, comment.Content);
        Assert.Equal(activityId, comment.ActivityId);
        Assert.Equal(userId, comment.UserId);
    }
}
