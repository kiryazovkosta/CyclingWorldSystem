namespace Domain.Entities
{
	using Domain.Errors;
	using Domain.Identity;
	using Domain.Primitives;
	using Domain.Shared;

	public sealed class Bike : DeletableEntity
	{
		private Bike(string brand, string model) 
			: base()
		{ 
			this.Brand = brand;
			this.Model = model;
		}

		private Bike() 
		{ 
			this.Activities = new HashSet<Activity>();
		}

		public string Name { get; init; } = null!;

		public Guid BikeTypeId { get; init; }
		public BikeType BikeType { get; init; } = null!;

		public decimal Weight { get; set; }

		public string Brand { get; init; } = null!;

		public string Model { get; init; } = null!;

		public string? Notes { get; init; }

		public Guid UserId { get; init; }
		public User User { get; init; } = null!;

		public ICollection<Activity> Activities { get; init; }

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