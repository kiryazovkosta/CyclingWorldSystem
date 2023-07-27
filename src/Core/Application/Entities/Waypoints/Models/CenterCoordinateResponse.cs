// ------------------------------------------------------------------------------------------------
//  <copyright file="CenterCoordinateResponse.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Waypoints.Models;

public sealed record CenterCoordinateResponse(
    decimal Longitude, 
    decimal Latitude, 
    decimal LongitudeDistance, 
    decimal LatitudeDistance);