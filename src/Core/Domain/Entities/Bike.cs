namespace Domain.Entities
{
	using Domain.Primitives;

	public sealed class Bike : Entity
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

		public static Bike Create(string brand, string model)
		{
			var bike = new Bike(brand, model);
			return bike;
		}
	}
}