// ------------------------------------------------------------------------------------------------
//  <copyright file="WaypointResponse.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Waypoints.Models;

public sealed record CoordinateResponse(decimal Latitude, decimal Longitude);