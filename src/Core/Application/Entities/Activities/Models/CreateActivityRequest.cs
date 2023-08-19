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