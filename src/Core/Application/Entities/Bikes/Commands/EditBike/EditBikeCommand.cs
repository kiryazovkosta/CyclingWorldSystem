namespace Application.Entities.Bikes.Commands.EditBike;

using Application.Abstractions.Messaging;
using MediatR;
using System;

public sealed record EditBikeCommand(Guid Id, string Brand, string Model) : ICommand<Unit>;
