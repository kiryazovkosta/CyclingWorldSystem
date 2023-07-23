// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateActivityRequest.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Activities.Models;

using Common.Enumerations;

public sealed record CreateActivityRequest(
    string Title,
    string Description,
    string? PrivateNotes,
    decimal Distance,
    TimeSpan Duration,
    decimal? PositiveElevation,
    decimal? NegativeElevation,
    VisibilityLevelType VisibilityLevel, 
    DateTime StartDateTime, 
    Guid BikeId, 
    List<string> Pictures, 
    Guid GpxId);