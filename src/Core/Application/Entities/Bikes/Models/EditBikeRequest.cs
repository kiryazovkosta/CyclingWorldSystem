namespace Application.Entities.Bikes.Models;

public sealed record EditBikeRequest(Guid Id, string Brand, string Model);
