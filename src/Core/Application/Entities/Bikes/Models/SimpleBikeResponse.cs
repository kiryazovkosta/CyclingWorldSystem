namespace Application.Entities.Bikes.Models;

public sealed record SimpleBikeResponse(
    Guid Id, string Brand, string Model);