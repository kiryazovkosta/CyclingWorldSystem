namespace Application.Entities.Bikes.Commands.CreateBike;

using Application.Abstractions.Messaging;
using Domain.Entities;

public sealed record CreateBikeCommand(string Brand, string Model) : ICommand<Bike>;