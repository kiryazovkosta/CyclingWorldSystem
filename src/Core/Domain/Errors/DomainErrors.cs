using Domain.Shared;

namespace Domain.Errors;

public static class DomainErrors
{
	public static class BikeType
	{
		public static Error BikeTypesCollectionIsNull => new(
		"BikeType.GetAllAsync",
		"The bike type collection is null!");
	}

	public static class Bike
	{
		public static Error BrandIsNullOrEmpty => new(
			"Bike.Create.Brand",
			"The brand can't be null or empty");

		public static Error ModelIsNullOrEmpty => new(
			"Bike.Create.Model",
			"The model can't be null or empty");

		public static Error BikeDoesNotExists(Guid id) => new(
			"Bike.Id",
			$"The bike with provided Id {id} does not exists.");
	}
}
