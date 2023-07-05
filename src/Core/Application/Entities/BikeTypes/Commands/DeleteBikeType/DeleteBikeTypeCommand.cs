namespace Application.Entities.BikeTypes.Commands.DeleteBikeType;

using Application.Abstractions.Messaging;
using System;

public sealed record DeleteBikeTypeCommand(Guid Id) : ICommand;