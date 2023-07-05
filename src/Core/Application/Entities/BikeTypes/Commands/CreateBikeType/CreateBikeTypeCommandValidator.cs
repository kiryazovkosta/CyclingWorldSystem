namespace Application.Entities.BikeTypes.Commands.CreateBikeType;

using Common.Constants;
using FluentValidation;

public class CreateBikeTypeCommandValidator : AbstractValidator<CreateBikeTypeCommand>
{
	public CreateBikeTypeCommandValidator()
	{
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