namespace Application.Entities.Bikes.Commands.CreateBike;

using Common.Constants;
using FluentValidation;

public class CreateBikeCommandValidator : AbstractValidator<CreateBikeCommand>
{
	public CreateBikeCommandValidator()
	{
		this.RuleFor(c => c.Name)
			.NotNull()
			.NotEmpty()
			.WithMessage(GlobalMessages.Bike.NameIsNullOrEmpty);

		this.RuleFor(c => c.Name)
			.MinimumLength(GlobalConstants.Bike.NameMinLength)
			.MaximumLength(GlobalConstants.Bike.NameMaxLength)
			.WithMessage(string.Format(
				GlobalMessages.Bike.NameLengthIsInvalid,
				GlobalConstants.Bike.NameMinLength,
				GlobalConstants.Bike.NameMaxLength));

		this.RuleFor(c => c.BikeTypeId)
			.NotNull()
			.NotEmpty()
			.NotEqual(Guid.Empty)
			.WithMessage(GlobalMessages.Bike.BikeTypeIsNullOrDefault);

		this.RuleFor(c => c.Weight)
			.LessThanOrEqualTo(GlobalConstants.Bike.WeightMaxValue)
			.GreaterThanOrEqualTo(GlobalConstants.Bike.WeightMinValue)
			.WithMessage(string.Format(
				GlobalMessages.Bike.WeightValueIsInvalid,
				GlobalConstants.Bike.WeightMinValue,
				GlobalConstants.Bike.WeightMaxValue));

		this.RuleFor(c => c.Brand)
			.NotNull()
			.NotEmpty()
			.WithMessage(GlobalMessages.Bike.BrandIsNullOrEmpty);

		this.RuleFor(c => c.Brand)
			.MinimumLength(GlobalConstants.Bike.BrandMinLength)
			.MaximumLength(GlobalConstants.Bike.BrandMaxLength)
			.WithMessage(string.Format(
				GlobalMessages.Bike.BrandLengthIsInvalid,
				GlobalConstants.Bike.BrandMinLength,
				GlobalConstants.Bike.BrandMaxLength));

		this.RuleFor(c => c.Model)
			.NotNull()
			.NotEmpty()
			.WithMessage(GlobalMessages.Bike.ModelIsNullOrEmpty);

		this.RuleFor(c => c.Model)
			.MinimumLength(GlobalConstants.Bike.ModelMinLength)
			.MaximumLength(GlobalConstants.Bike.ModelMaxLength)
			.WithMessage(string.Format(
				GlobalMessages.Bike.ModelLengthIsInvalid,
				GlobalConstants.Bike.ModelMinLength,
				GlobalConstants.Bike.ModelMaxLength));

		this.RuleFor(c => c.Notes)
			.MaximumLength(GlobalConstants.Bike.NotesMaxLength)
			.WithMessage(string.Format(
				GlobalMessages.Bike.NotesLengthIsInvalid,
				GlobalConstants.Bike.NotesMaxLength));
	}
}
