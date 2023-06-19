namespace Domain.Entities
{
	using Domain.Errors;
	using Domain.Primitives;
	using Domain.Shared;

	public sealed class Bike : DeletetableEntity
	{
		private Bike(string brand, string model) 
			: base()
		{ 
			this.Brand = brand;
			this.Model = model;
		}

		private Bike() 
		{ 
		}

		public string Brand { get; init; } = null!;

		public string Model { get; init; } = null!;

		public static Result<Bike> Create(string brand, string model)
		{
			if (string.IsNullOrEmpty(brand))
			{
				return Result.Failure<Bike>(DomainErrors.Bike.BrandIsNullOrEmpty);
			}

			if (string.IsNullOrEmpty(model))
			{
				return Result.Failure<Bike>(DomainErrors.Bike.ModelIsNullOrEmpty);
			}

			var bike = new Bike(brand, model);
			return bike;
		}
	}
}