// ------------------------------------------------------------------------------------------------
//  <copyright file="GeoCoordinateServiceTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Services;

using Application.Services;

public class GeoCoordinateServiceTests
{
    [Theory]
    [InlineData(0, 0, 0, 0, 0)] // Same coordinates
    [InlineData(0, 0, 0, 180, 20032366)] // Opposite sides of the earth
    [InlineData(0, 0, 90, 0, 10016183)] // North pole to equator
    [InlineData(0, 0, -90, 0, 10016183)] // South pole to equator
    [InlineData(0, 0, 90, 180, 10016183)] // North pole to antimeridian
    [InlineData(0, 0, -90, 180, 10016183)] // South pole to antimeridian
    [InlineData(52.5200, 13.4050, 37.7749, -122.4194, 9113066)] // Berlin to San Francisco
    public async Task GetDistance_ShouldCalculateCorrectDistance(
        decimal latitude, decimal longitude,
        decimal otherLatitude, decimal otherLongitude, double expectedDistance)
    {
        // Arrange
        var geoCoordinate = new GeoCoordinate();

        // Act
        var distance = await geoCoordinate.GetDistance(longitude, latitude, otherLongitude, otherLatitude);

        // Assert
        Assert.Equal(expectedDistance, distance, precision: 0);
    }
}