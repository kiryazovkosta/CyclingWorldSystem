// ------------------------------------------------------------------------------------------------
//  <copyright file="WaypointCoordinate.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Models.Waypoints;

public sealed record WaypointCoordinate(decimal Latitude, decimal Longitude);