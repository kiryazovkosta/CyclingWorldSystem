using Domain.Entities.GpxFile;

namespace Tests.DomainTests.GpxFile;

public class GpxTrkTrkptTests
{
    [Fact]
    public void GpxTrkTrkpt_Latitude_SetAndGet()
    {
        // Arrange
        var gpxTrkTrkpt = new GpxTrkTrkpt();
        var latitude = 42.12345m;

        // Act
        gpxTrkTrkpt.Latitude = latitude;

        // Assert
        Assert.Equal(latitude, gpxTrkTrkpt.Latitude);
    }

    [Fact]
    public void GpxTrkTrkpt_Longitude_SetAndGet()
    {
        // Arrange
        var gpxTrkTrkpt = new GpxTrkTrkpt();
        var longitude = -71.98765m;

        // Act
        gpxTrkTrkpt.Longitude = longitude;

        // Assert
        Assert.Equal(longitude, gpxTrkTrkpt.Longitude);
    }

    [Fact]
    public void GpxTrkTrkpt_Elevation_SetAndGet()
    {
        // Arrange
        var gpxTrkTrkpt = new GpxTrkTrkpt();
        var elevation = 100.0m;

        // Act
        gpxTrkTrkpt.Elevation = elevation;

        // Assert
        Assert.Equal(elevation, gpxTrkTrkpt.Elevation);
    }

    [Fact]
    public void GpxTrkTrkpt_Time_SetAndGet()
    {
        // Arrange
        var gpxTrkTrkpt = new GpxTrkTrkpt();
        var time = DateTime.Now;

        // Act
        gpxTrkTrkpt.Time = time;

        // Assert
        Assert.Equal(time, gpxTrkTrkpt.Time);
    }

    [Fact]
    public void GpxTrkTrkpt_Extensions_SetAndGet()
    {
        // Arrange
        var gpxTrkTrkpt = new GpxTrkTrkpt();
        var extensions = new GpxTrkTrkptExtensions();

        // Act
        gpxTrkTrkpt.Extensions = extensions;

        // Assert
        Assert.Same(extensions, gpxTrkTrkpt.Extensions);
    }
}
