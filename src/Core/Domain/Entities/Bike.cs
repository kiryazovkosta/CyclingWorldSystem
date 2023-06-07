namespace Domain.Entities
{
	using Domain.Primitives;
	using System;

	public sealed class Bike : Entity
	{
		public Bike(Guid id, string brand, string model) 
			: base(id)
		{ 
			this.Brand = brand;
			this.Model = model;
		}

		private Bike() 
		{ 
		}

		public string Brand { get; init; } = null!;

		public string Model { get; init; } = null!;
	}
}