

using Domain.Entities;
using Domain.Identity;

namespace Tests.DomainTests.Entities;

public class ActivityLikeTests
{
    [Fact]
    public void ActivityLike_SetAndGetActivityId()
    {
        // Arrange
        var activityLike = new ActivityLike();
        var activityId = Guid.NewGuid();

        // Act
        activityLike.ActivityId = activityId;
        var retrievedActivityId = activityLike.ActivityId;

        // Assert
        Assert.Equal(activityId, retrievedActivityId);
    }

    [Fact]
    public void ActivityLike_SetAndGetActivity()
    {
        // Arrange
        var activityLike = new ActivityLike();
        var activity = Activity.Create("title", "desc", null, 10m, new TimeSpan(10), null, null, 
            Common.Enumerations.VisibilityLevelType.All, DateTime.Now, Guid.NewGuid(), Guid.NewGuid()).Value;

        // Act
        activityLike.Activity = activity;
        var retrievedActivity = activityLike.Activity;

        // Assert
        Assert.Same(activity, retrievedActivity);
    }

    [Fact]
    public void ActivityLike_SetAndGetUserId()
    {
        // Arrange
        var activityLike = new ActivityLike();
        var userId = Guid.NewGuid();

        // Act
        activityLike.UserId = userId;
        var retrievedUserId = activityLike.UserId;

        // Assert
        Assert.Equal(userId, retrievedUserId);
    }

    [Fact]
    public void ActivityLike_SetAndGetUser()
    {
        // Arrange
        var activityLike = new ActivityLike();
        var user = new User();

        // Act
        activityLike.User = user;
        var retrievedUser = activityLike.User;

        // Assert
        Assert.Same(user, retrievedUser);
    }
}
