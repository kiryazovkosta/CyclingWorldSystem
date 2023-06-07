namespace Application.Entities.Bikes.Commands.CreateBike
{
	using Application.Abstractions.Messaging;
	using System;

	public sealed record CreateBikeCommand(string Brand, string Model) : ICommand<Guid>
	{
	}
}