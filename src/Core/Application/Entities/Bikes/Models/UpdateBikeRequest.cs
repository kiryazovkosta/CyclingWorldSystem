namespace Application.Entities.Bikes.Models;

public sealed record UpdateBikeRequest(
	Guid Id, 
	string Name, 
	Guid BikeTypeId, 
	decimal Weight, 
	string Brand, 
	string Model, 
	string? Notes);
