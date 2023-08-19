namespace Application.Entities.Activities.Models;

public sealed record SimplyActivityResponse(
    Guid Id,
    string UserName,
    string Title,
    string Description,
    decimal Distance,
    decimal PositiveElevation,
    TimeSpan Duration,
    string Avatar,
    string FirstPicture);