using Domain.Entities.GpxFile;

namespace Tests.DomainTests.GpxFile;

public class TrackPointExtensionTests
{
    [Fact]
    public void TrackPointExtension_Temperature_SetAndGet()
    {
        // Arrange
        var trackPointExtension = new TrackPointExtension();
        var temperature = 25.5m;

        // Act
        trackPointExtension.Temperature = temperature;

        // Assert
        Assert.Equal(temperature, trackPointExtension.Temperature);
    }

    [Fact]
    public void TrackPointExtension_HeartRate_SetAndGet()
    {
        // Arrange
        var trackPointExtension = new TrackPointExtension();
        var heartRate = (byte)80;

        // Act
        trackPointExtension.HeartRate = heartRate;

        // Assert
        Assert.Equal(heartRate, trackPointExtension.HeartRate);
    }

    [Fact]
    public void TrackPointExtension_Cadance_SetAndGet()
    {
        // Arrange
        var trackPointExtension = new TrackPointExtension();
        var cadance = (byte)90;

        // Act
        trackPointExtension.Cadance = cadance;

        // Assert
        Assert.Equal(cadance, trackPointExtension.Cadance);
    }
}
