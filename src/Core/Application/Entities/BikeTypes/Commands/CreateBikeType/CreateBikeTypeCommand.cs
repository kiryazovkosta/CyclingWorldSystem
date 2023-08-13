namespace Application.Entities.BikeTypes.Commands.CreateBikeType;

using Application.Abstractions.Messaging;
using Application.Entities.BikeTypes.Models;

public sealed record CreateBikeTypeCommand(string Name) : ICommand<Guid>;