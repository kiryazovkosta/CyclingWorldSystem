// ------------------------------------------------------------------------------------------------
//  <copyright file="WaypointCenterCoordinate.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Models.Waypoints;

public sealed record WaypointCenterCoordinate(
    decimal Longitude, 
    decimal Latitude, 
    decimal LongitudeDistance, 
    decimal LatitudeDistance);