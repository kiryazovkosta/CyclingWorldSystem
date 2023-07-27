// ------------------------------------------------------------------------------------------------
//  <copyright file="GetActivityCenterCoordinateQuery.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Waypoints.Queries.GetActivityCenterCoordinate;

using Abstractions.Messaging;
using Models;

public sealed record GetActivityCenterCoordinateQuery(Guid ActivityId) : IQuery<CenterCoordinateResponse>;