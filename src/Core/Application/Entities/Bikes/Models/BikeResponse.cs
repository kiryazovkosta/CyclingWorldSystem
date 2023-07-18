namespace Application.Entities.Bikes.Models;

public sealed record BikeResponse(
    Guid Id, 
    string Name, 
    Guid BikeTypeId, 
    string BikeType, 
    decimal Weight, 
    string Brand, 
    string Model, 
    string? Notes, 
    Guid UserId);