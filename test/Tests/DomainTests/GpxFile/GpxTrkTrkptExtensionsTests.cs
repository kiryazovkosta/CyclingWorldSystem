using Domain.Entities.GpxFile;

namespace Tests.DomainTests.GpxFile;

public class GpxTrkTrkptExtensionsTests
{
    [Fact]
    public void GpxTrkTrkptExtensions_Power_SetAndGet()
    {
        // Arrange
        var gpxTrkTrkptExtensions = new GpxTrkTrkptExtensions();
        var power = (ushort)150;

        // Act
        gpxTrkTrkptExtensions.Power = power;

        // Assert
        Assert.Equal(power, gpxTrkTrkptExtensions.Power);
    }

    [Fact]
    public void GpxTrkTrkptExtensions_TrackPointExtension_SetAndGet()
    {
        // Arrange
        var gpxTrkTrkptExtensions = new GpxTrkTrkptExtensions();
        var trackPointExtension = new TrackPointExtension();

        // Act
        gpxTrkTrkptExtensions.TrackPointExtension = trackPointExtension;

        // Assert
        Assert.Same(trackPointExtension, gpxTrkTrkptExtensions.TrackPointExtension);
    }
}
