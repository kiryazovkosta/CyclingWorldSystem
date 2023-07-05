namespace Application.Entities.Bikes.Commands.CreateBike;

using Application.Abstractions.Messaging;
using Domain.Entities;

public sealed record CreateBikeCommand(
	string Name, 
	Guid BikeTypeId, 
	decimal Weight, 
	string Brand, 
	string Model, 
	string? Notes) : ICommand<Bike>;