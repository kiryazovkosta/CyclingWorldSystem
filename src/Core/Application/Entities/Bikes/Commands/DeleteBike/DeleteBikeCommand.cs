namespace Application.Entities.Bikes.Commands.DeleteBike
{
	using Application.Abstractions.Messaging;
	using MediatR;

	public sealed record DeleteBikeCommand(Guid Id) : ICommand;
}