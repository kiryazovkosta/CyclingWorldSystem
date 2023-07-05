namespace Application.Entities.Bikes.Models;

using System.ComponentModel;

public sealed record CreateBikeRequest(
	string Name, 
	Guid BikeTypeId, 
	decimal Weight,
	string Brand, 
	string Model,
	string? Notes
);