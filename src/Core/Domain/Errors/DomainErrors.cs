using Domain.Shared;

namespace Domain.Errors;

public static class DomainErrors
{
	public static class Bike
	{
		public static readonly Error BrandIsNullOrEmpty = new(
			"Bike.Create.Brand",
			"The brand can't be null or empty");

		public static readonly Error ModelIsNullOrEmpty = new(
			"Bike.Create.Model",
			"The model can't be null or empty");
	}
}
