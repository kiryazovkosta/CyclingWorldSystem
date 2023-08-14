// ------------------------------------------------------------------------------------------------
//  <copyright file="GpxServiceTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Services;

using Application.Services;

public class GpxServiceTests
{
    [Fact]
        public async Task Get_DeserializeXml_Success()
        {
            // Arrange
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                        <gpx creator=""StravaGPX"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd http://www.garmin.com/xmlschemas/GpxExtensions/v3 http://www.garmin.com/xmlschemas/GpxExtensionsv3.xsd http://www.garmin.com/xmlschemas/TrackPointExtension/v1 http://www.garmin.com/xmlschemas/TrackPointExtensionv1.xsd"" version=""1.1"" xmlns=""http://www.topografix.com/GPX/1/1"" xmlns:gpxtpx=""http://www.garmin.com/xmlschemas/TrackPointExtension/v1"" xmlns:gpxx=""http://www.garmin.com/xmlschemas/GpxExtensions/v3"">
                            <metadata>
                                <time>2022-08-08T14:42:36Z</time>
                            </metadata>
                            <trk>
                                <name>Afternoon Mountain Bike Ride</name>
                                <trkseg>
                                    <trkpt lat=""42.5113420"" lon=""27.4724860"">
                                        <ele>90.8</ele>
                                        <time>2022-08-08T14:42:36Z</time>
                                        <extensions>
                                            <gpxtpx:TrackPointExtension>
                                                <gpxtpx:atemp>29</gpxtpx:atemp>
                                            </gpxtpx:TrackPointExtension>
                                        </extensions>
                                    </trkpt>
                                </trkseg>
                            </trk>
                        </gpx>";

            var service = new GpxService();

            // Act
            var result = await service.Get(xml);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Metadata);
            Assert.NotNull(result.Trk);
            Assert.Single(result.Trk.Trkseg);
        }
}