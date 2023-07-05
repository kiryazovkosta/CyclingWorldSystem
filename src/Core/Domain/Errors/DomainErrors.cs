using Common.Constants;
using Domain.Shared;

namespace Domain.Errors;

public static class DomainErrors
{
	public static Error DeleteOperationFailed(Guid id, string command) => new(
		command,
		$"There is a error with deleting product type with Id {id}");


	public static class BikeType
	{
		public static Error BikeTypesCollectionIsNull => new(
			"BikeType.GetAllAsync",
			"The bike type collection is null!");

		public static Error BikeTypeNameIsNull => new(
			"BikeType.Create.Name",
			GlobalMessages.BikeType.NameIsNullOrEmpty);

		public static Error BikeTypeNameInvalidLength(int min, int max) => new(
			"BikeType.Create.Name",
			string.Format(GlobalMessages.BikeType.NameLengthIsInvalid, min, max));

		public static Error BikeTypeNameExists(string name) => new(
			"BikeType.Name",
			string.Format($"There is a bike type with name: {0}", name));

		public static Error BikeTypeDoesNotExists => new(
			"BikeType.Update",
			"The bike type with provided identifier does not exists!");
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
