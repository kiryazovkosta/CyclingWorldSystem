using Domain.Entities;

namespace Tests.DomainTests.Entities;

public class ActivityTests
{
    [Fact]
    public void Activity_Constructors_InitializePropertiesAndCollections()
    {
        // Arrange & Act
        var activity = Activity.Create("title", "description", null, 10m, new TimeSpan(10), null, null, 
            Common.Enumerations.VisibilityLevelType.All, DateTime.Now, Guid.NewGuid(), Guid.NewGuid()).Value;

        // Assert
        Assert.NotNull(activity.Waypoints);
        Assert.IsType<HashSet<Waypoint>>(activity.Waypoints);

        Assert.NotNull(activity.Images);
        Assert.IsType<HashSet<Image>>(activity.Images);

        Assert.NotNull(activity.Likes);
        Assert.IsType<HashSet<ActivityLike>>(activity.Likes);

        Assert.NotNull(activity.Comments);
        Assert.IsType<HashSet<Comment>>(activity.Comments);
    }

    [Fact]
    public void Activity_Title_SetAndGet()
    {
        // Arrange
        var activity = Activity.Create("title", "description", null, 10m, new TimeSpan(10), null, null, 
            Common.Enumerations.VisibilityLevelType.All, DateTime.Now, Guid.NewGuid(), Guid.NewGuid()).Value;
        var title = "Test Activity";

        // Act
        activity.Title = title;

        // Assert
        Assert.Equal(title, activity.Title);
    }

    [Fact]
    public void Activity_Description_SetAndGet()
    {
        // Arrange
        var activity = Activity.Create("title", "description", null, 10m, new TimeSpan(10), null, null, 
            Common.Enumerations.VisibilityLevelType.All, DateTime.Now, Guid.NewGuid(), Guid.NewGuid()).Value;
        var description = "Test activity description.";

        // Act
        activity.Description = description;

        // Assert
        Assert.Equal(description, activity.Description);
    }

    [Fact]
    public void Activity_AddImages_NullOrEmptyPictures_Success()
    {
        // Arrange
        var activity = Activity.Create("title", "description", null, 10m, new TimeSpan(10), null, null,
            Common.Enumerations.VisibilityLevelType.All, DateTime.Now, Guid.NewGuid(), Guid.NewGuid()).Value;
        List<string>? pictures = null;

        // Act
        var result = activity.AddImages(pictures);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(activity.Images);
    }

    [Fact]
    public void Activity_AddImages_AddsImagesToList()
    {
        // Arrange
        var activity = Activity.Create("title", "description", null, 10m, new TimeSpan(10), null, null,
            Common.Enumerations.VisibilityLevelType.All, DateTime.Now, Guid.NewGuid(), Guid.NewGuid()).Value;
        var pictures = new List<string> { "image1.jpg", "image2.jpg" };

        // Act
        var result = activity.AddImages(pictures);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(pictures.Count, activity.Images.Count);
        Assert.Equal(pictures[0], activity.Images.ElementAt(0).Url);
        Assert.Equal(pictures[1], activity.Images.ElementAt(1).Url);
    }
}
