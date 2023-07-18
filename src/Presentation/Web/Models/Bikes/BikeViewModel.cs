namespace Web.Models.Bikes;

public sealed record BikeViewModel(
    Guid Id,
    string Name,
    Guid BikeTypeId,
    string BikeType,
    decimal Weight,
    string Brand,
    string Model,
    string? Notes,
    Guid UserId);