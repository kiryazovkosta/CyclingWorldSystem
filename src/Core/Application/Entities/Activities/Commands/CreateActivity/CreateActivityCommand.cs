// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateActivityCommand.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Activities.Commands.CreateActivity;

using Abstractions.Messaging;
using Common.Enumerations;

public sealed record CreateActivityCommand(
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
    Guid GpxId) : ICommand<Guid>;