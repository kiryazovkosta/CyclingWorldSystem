using FluentValidation;

namespace Application.Entities.Bikes.Commands.DeleteBike;

public sealed class DeleteBikeCommandValidator : AbstractValidator<DeleteBikeCommand>
{
	public DeleteBikeCommandValidator()
	{
	}
}
