namespace Application.Entities.Bikes.Commands.UpdateBike;

using Application.Abstractions.Messaging;
using System;

public sealed record UpdateBikeCommand(
	Guid Id,
	string Name,
	Guid BikeTypeId,
	decimal Weight,
	string Brand,
	string Model,
	string? Notes) 
	: ICommand;