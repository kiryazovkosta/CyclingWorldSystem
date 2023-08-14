using Domain.Entities.GpxFile;

namespace Tests.DomainTests.GpxFile;

public class GpxMetadataTests
{
    [Fact]
    public void GpxMetadata_Name_SetAndGet()
    {
        // Arrange
        var gpxMetadata = new GpxMetadata();
        var name = "Test Metadata";

        // Act
        gpxMetadata.Name = name;

        // Assert
        Assert.Equal(name, gpxMetadata.Name);
    }

    [Fact]
    public void GpxMetadata_Time_SetAndGet()
    {
        // Arrange
        var gpxMetadata = new GpxMetadata();
        var time = DateTime.Now;

        // Act
        gpxMetadata.Time = time;

        // Assert
        Assert.Equal(time, gpxMetadata.Time);
    }
}
