namespace Application.Entities.BikeTypes.Commands.UpdateBikeType;

using Application.Abstractions.Messaging;

public sealed record UpdateBikeTypeCommand(Guid Id, string Name) : ICommand;