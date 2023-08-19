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