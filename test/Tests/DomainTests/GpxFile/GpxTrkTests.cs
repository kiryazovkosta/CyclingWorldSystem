using Domain.Entities.GpxFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.DomainTests.GpxFile;

public class GpxTrkTests
{
    [Fact]
    public void GpxTrk_Name_SetAndGet()
    {
        // Arrange
        var gpxTrk = new GpxTrk();
        var name = "Test Track";

        // Act
        gpxTrk.Name = name;

        // Assert
        Assert.Equal(name, gpxTrk.Name);
    }

    [Fact]
    public void GpxTrk_Type_SetAndGet()
    {
        // Arrange
        var gpxTrk = new GpxTrk();
        var type = (byte)2;

        // Act
        gpxTrk.Type = type;

        // Assert
        Assert.Equal(type, gpxTrk.Type);
    }

    [Fact]
    public void GpxTrk_Description_SetAndGet()
    {
        // Arrange
        var gpxTrk = new GpxTrk();
        var description = "Test track description.";

        // Act
        gpxTrk.Description = description;

        // Assert
        Assert.Equal(description, gpxTrk.Description);
    }

    [Fact]
    public void GpxTrk_Trkseg_SetAndGet()
    {
        // Arrange
        var gpxTrk = new GpxTrk();
        var trkseg = new GpxTrkTrkpt[] { new GpxTrkTrkpt(), new GpxTrkTrkpt() };

        // Act
        gpxTrk.Trkseg = trkseg;

        // Assert
        Assert.Same(trkseg, gpxTrk.Trkseg);
    }
}
