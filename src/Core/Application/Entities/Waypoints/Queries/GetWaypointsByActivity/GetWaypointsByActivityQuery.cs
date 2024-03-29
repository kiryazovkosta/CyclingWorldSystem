﻿// ------------------------------------------------------------------------------------------------
//  <copyright file="GetWaypointsByActivityQuery.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Waypoints.Queries.GetWaypointsByActivity;

using Abstractions.Messaging;
using Models;

public sealed record GetWaypointsByActivityQuery(Guid Id) : IQuery<IEnumerable<CoordinateResponse>>;