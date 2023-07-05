namespace Application.Entities.Bikes.Commands.UpdateBike;

using FluentValidation;
using System;

public class UpdateBikeCommandValidator : AbstractValidator<UpdateBikeCommand>
{
	public UpdateBikeCommandValidator()
	{
		this.RuleFor(c => c.Id).NotEqual(Guid.Empty);
		this.RuleFor(c => c.Brand).NotEmpty();
		this.RuleFor(c => c.Model).NotEmpty();
	}
}