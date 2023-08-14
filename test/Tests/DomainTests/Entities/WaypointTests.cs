using Domain.Entities;

namespace Tests.DomainTests.Entities;

public class WaypointTests
{
    [Fact]
    public void Waypoint_Create_ValidData_ReturnsWaypoint()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        var orderIndex = 1;
        var latitude = 42.12345m;
        var longitude = -71.98765m;
        var elevation = 100.0m;
        var time = DateTime.Now;
        var temperature = 25.5m;
        var heartRate = 75;
        var power = 200;
        var speed = 12.5m;
        var gpxId = Guid.NewGuid();

        // Act
        var result = Waypoint.Create(activityId, orderIndex, latitude, longitude, elevation,
            time, temperature, heartRate, power, speed, gpxId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(activityId, result.Value.ActivityId);
        Assert.Equal(orderIndex, result.Value.OrderIndex);
        Assert.Equal(latitude, result.Value.Latitude);
        Assert.Equal(longitude, result.Value.Longitude);
        Assert.Equal(elevation, result.Value.Elevation);
        Assert.Equal(time, result.Value.Time);
        Assert.Equal(temperature, result.Value.Temperature);
        Assert.Equal(heartRate, result.Value.HeartRate);
        Assert.Equal(power, result.Value.Power);
        Assert.Equal(speed, result.Value.Speed);
        Assert.Equal(gpxId, result.Value.GpxId);
    }
}
