namespace Application.Entities.Bikes.Commands.CreateBike;

using FluentValidation;

public class CreateBikeCommandValidator : AbstractValidator<CreateBikeCommand>
{
	public CreateBikeCommandValidator()
	{
		this.RuleFor(c => c.Brand).NotEmpty();
		this.RuleFor(c => c.Model).NotEmpty();
	}
}
