namespace Application.Entities.BikeTypes.Commands.UpdateBikeType;

using Common.Constants;
using FluentValidation;
using System;

public sealed class UpdateBikeTypeCommandValidator : AbstractValidator<UpdateBikeTypeCommand>
{
	public UpdateBikeTypeCommandValidator()
	{
		this.RuleFor(bt => bt.Id)
			.NotEmpty()
			.NotEqual(Guid.Empty)
			.WithMessage(GlobalMessages.UniqueIdentifierIsNullOrDefault);

		this.RuleFor(c => c.Name)
			.NotNull()
			.NotEmpty()
			.WithMessage(GlobalMessages.BikeType.NameIsNullOrEmpty);

		this.RuleFor(c => c.Name)
			.MinimumLength(GlobalConstants.BikeType.NameMinLength)
			.MaximumLength(GlobalConstants.BikeType.NameMaxLength)
			.WithMessage(string.Format(
				GlobalMessages.BikeType.NameLengthIsInvalid,
				GlobalConstants.BikeType.NameMinLength,
				GlobalConstants.BikeType.NameMaxLength));
	}
}