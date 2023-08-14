using Domain.Entities.GpxFile;

namespace Tests.DomainTests.GpxFile;

public class GpxTests
{
    [Fact]
    public void Gpx_Metadata_SetAndGet()
    {
        // Arrange
        var gpx = new Gpx();
        var metadata = new GpxMetadata();

        // Act
        gpx.Metadata = metadata;

        // Assert
        Assert.Same(metadata, gpx.Metadata);
    }

    [Fact]
    public void Gpx_Trk_SetAndGet()
    {
        // Arrange
        var gpx = new Gpx();
        var trk = new GpxTrk();

        // Act
        gpx.Trk = trk;

        // Assert
        Assert.Same(trk, gpx.Trk);
    }
}
