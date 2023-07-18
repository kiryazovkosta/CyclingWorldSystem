namespace Domain.Entities.Dtos;

public sealed record BikeResponseDto(
    Guid Id,
    string Name,
    Guid BikeTypeId,
    string BikeType,
    decimal Weight,
    string Brand,
    string Model,
    string? Notes,
    Guid UserId);