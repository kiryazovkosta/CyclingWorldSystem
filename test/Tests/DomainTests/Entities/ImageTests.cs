using Domain.Entities;

namespace Tests.DomainTests.Entities;

public class ImageTests
{
    [Fact]
    public void Image_Create_ValidData_ReturnsImage()
    {
        // Arrange
        var url = "https://example.com/image.jpg";
        var activity = Activity.Create("title", "desc", null, 10m, new TimeSpan(10), null, null,
            Common.Enumerations.VisibilityLevelType.All, DateTime.Now, Guid.NewGuid(), Guid.NewGuid()).Value;

        // Act
        var result = Image.Create(url, activity);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(url, result.Value.Url);
        Assert.Same(activity, result.Value.Activity);
    }

    [Fact]
    public void Image_Url_SetAndGet()
    {
        // Arrange
        var url = "https://example.com/image.jpg";
        var activity = Activity.Create("title", "desc", null, 10m, new TimeSpan(10), null, null,
            Common.Enumerations.VisibilityLevelType.All, DateTime.Now, Guid.NewGuid(), Guid.NewGuid()).Value;
        var image = Image.Create(url, activity).Value;

        // Act
        image.Url = url;

        // Assert
        Assert.Equal(url, image.Url);
    }

    [Fact]
    public void Image_ActivityId_SetAndGet()
    {
        // Arrange
        var url = "https://example.com/image.jpg";
        var activity = Activity.Create("title", "desc", null, 10m, new TimeSpan(10), null, null,
            Common.Enumerations.VisibilityLevelType.All, DateTime.Now, Guid.NewGuid(), Guid.NewGuid()).Value;
        var image = Image.Create(url, activity).Value;
        var activityId = Guid.NewGuid();

        // Act
        image.ActivityId = activityId;

        // Assert
        Assert.Equal(activityId, image.ActivityId);
    }

    [Fact]
    public void Image_Activity_SetAndGet()
    {
        // Arrange
        var url = "https://example.com/image.jpg";
        var activity = Activity.Create("title", "desc", null, 10m, new TimeSpan(10), null, null,
            Common.Enumerations.VisibilityLevelType.All, DateTime.Now, Guid.NewGuid(), Guid.NewGuid()).Value;
        var image = Image.Create(url, activity).Value;

        // Act
        image.Activity = activity;

        // Assert
        Assert.Same(activity, image.Activity);
    }
}
