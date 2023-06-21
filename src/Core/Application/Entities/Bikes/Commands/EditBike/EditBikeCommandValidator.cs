namespace Application.Entities.Bikes.Commands.EditBike;

using FluentValidation;
using System;

public class EditBikeCommandValidator : AbstractValidator<EditBikeCommand>
{
	public EditBikeCommandValidator()
	{
		this.RuleFor(c => c.Id).NotEqual(Guid.Empty);
		this.RuleFor(c => c.Brand).NotEmpty();
		this.RuleFor(c => c.Model).NotEmpty();
	}
}