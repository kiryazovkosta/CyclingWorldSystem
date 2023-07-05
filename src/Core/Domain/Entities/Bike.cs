namespace Domain.Entities
{
	using Common.Constants;
	using Domain.Errors;
	using Domain.Identity;
	using Domain.Primitives;
	using Domain.Shared;

	public sealed class Bike : DeletableEntity
	{
		private Bike(string name, Guid bikeTypeId, decimal weight, string brand, string model, string? notes, Guid userId) 
			: this()
		{ 
			this.Name = name;
			this.BikeTypeId = bikeTypeId;
			this.Weight = weight;
			this.Brand = brand;
			this.Model = model;
			this.Notes = notes;
			this.UserId = userId;
		}

		private Bike() 
		{ 
			this.Activities = new HashSet<Activity>();
		}

		public string Name { get; private set; } = null!;

		public Guid BikeTypeId { get; private set; }
		public BikeType BikeType { get; init; } = null!;

		public decimal Weight { get; private set; }

		public string Brand { get; private set; } = null!;

		public string Model { get; private set; } = null!;

		public string? Notes { get; private set; }

		public Guid UserId { get; init; }
		public User User { get; init; } = null!;

		public ICollection<Activity> Activities { get; init; }

		public static Result<Bike> Create(
			string name,
			Guid bikeTypeId,
			decimal weight,
			string brand, 
			string model,
			string? notes,
			Guid userId)
		{
			var validateResult = Validate(name, bikeTypeId, weight, brand, model, notes);
			if (validateResult.IsFailure) 
			{
				return Result.Failure<Bike>(validateResult.Error);
			}

			var bike = new Bike(name, bikeTypeId, weight, brand, model, notes, userId);
			return bike;
		}

		public Result Update(
			string name, 
			Guid bikeTypeId, 
			decimal weight, 
			string brand, 
			string model, 
			string? notes,
			Guid userId)
		{
			if (this.UserId != userId)
			{
				return Result.Failure(DomainErrors.UnauthorizedAccess("Bike.Update"));
			}

			var validateResult = Validate(name, bikeTypeId, weight, brand, model, notes);
			if (validateResult.IsFailure)
			{
				return Result.Failure(validateResult.Error);
			}

			this.Name = name;
			this.BikeTypeId = bikeTypeId;
			this.Weight = weight;
			this.Brand = brand;
			this.Model = model;
			this.Notes = notes;
			return Result.Success();
		}

		private static Result ValidateName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return Result.Failure<Bike>(DomainErrors.Bike.NameIsNullOrEmpty);
			}

			if (name.Length < GlobalConstants.Bike.NameMinLength
				|| name.Length > GlobalConstants.Bike.NameMaxLength)
			{
				return Result.Failure<Bike>(
					DomainErrors.Bike.NameLengthIsInvalid(
						GlobalConstants.Bike.NameMinLength,
						GlobalConstants.Bike.NameMaxLength));
			}

			return Result.Success();
		}

		private static Result ValidateBikeType(Guid id)
		{
			if (id == Guid.Empty)
			{
				return Result.Failure<Bike>(DomainErrors.Bike.BikeTypeIsInvalid);
			}

			return Result.Success();
		}

		private static Result ValidateWeight(decimal weight)
		{
			if (weight < GlobalConstants.Bike.WeightMinValue
				|| weight > GlobalConstants.Bike.WeightMaxValue)
			{
				return Result.Failure<Bike>(
					DomainErrors.Bike.WeightIsInvalid(
						GlobalConstants.Bike.WeightMinValue, 
						GlobalConstants.Bike.WeightMaxValue));
			}

			return Result.Success();
		}

		private static Result ValidateBrand(string brand)
		{
			if (string.IsNullOrWhiteSpace(brand))
			{
				return Result.Failure<Bike>(DomainErrors.Bike.BrandIsNullOrEmpty);
			}

			if (brand.Length < GlobalConstants.Bike.BrandMinLength
				|| brand.Length > GlobalConstants.Bike.BrandMaxLength)
			{
				return Result.Failure<Bike>(
					DomainErrors.Bike.BrandLengthIsInvalid(
						GlobalConstants.Bike.BrandMinLength,
						GlobalConstants.Bike.BrandMaxLength));
			}

			return Result.Success();
		}

		private static Result ValidateModel(string model)
		{
			if (string.IsNullOrWhiteSpace(model))
			{
				return Result.Failure<Bike>(DomainErrors.Bike.ModelIsNullOrEmpty);
			}

			if (model.Length < GlobalConstants.Bike.ModelMinLength
				|| model.Length > GlobalConstants.Bike.ModelMaxLength)
			{
				return Result.Failure<Bike>(
					DomainErrors.Bike.ModelLengthIsInvalid(
						GlobalConstants.Bike.ModelMinLength,
						GlobalConstants.Bike.ModelMaxLength));
			}

			return Result.Success();
		}

		private static Result ValidateNotes(string? notes)
		{
			if (notes is not null 
				&& notes.Length > GlobalConstants.Bike.NotesMaxLength)
			{
				return Result.Failure<Bike>(
					DomainErrors.Bike.NotesLengthIsInvalid(
						GlobalConstants.Bike.NotesMaxLength));
			}

			return Result.Success();
		}

		public static Result Validate(
			string name, 
			Guid bikeTypeId, 
			decimal weight, 
			string brand,
			string model,
			string? notes) 
		{
			var nameValidationResult = ValidateName(name);
			if (nameValidationResult.IsFailure)
			{
				return Result.Failure<Bike>(nameValidationResult.Error);
			}

			var typeIdValidationResult = ValidateBikeType(bikeTypeId);
			if (typeIdValidationResult.IsFailure)
			{
				return Result.Failure<Bike>(typeIdValidationResult.Error);
			}

			var weightValidationResult = ValidateWeight(weight);
			if (weightValidationResult.IsFailure)
			{
				return Result.Failure<Bike>(weightValidationResult.Error);
			}

			var brandValidationResult = ValidateBrand(brand);
			if (brandValidationResult.IsFailure)
			{
				return Result.Failure<Bike>(brandValidationResult.Error);
			}

			var modelValidationResult = ValidateModel(model);
			if (modelValidationResult.IsFailure)
			{
				return Result.Failure<Bike>(modelValidationResult.Error);
			}

			var notesValidationResult = ValidateNotes(notes);
			if (notesValidationResult.IsFailure)
			{
				return Result.Failure<Bike>(notesValidationResult.Error);
			}

			return Result.Success();
		}
	}
}