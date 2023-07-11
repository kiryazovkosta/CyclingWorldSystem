namespace Common.Constants;

public static class GlobalMessages
{
	public const string UniqueIdentifierIsNullOrDefault = "The provided Identificator cloud not be null or defaul.";
	public const string GlobalError = "An unexpected error occurred during request execution.!";

	public static class GpxFile
	{
		public const string CollectionIsEmpty = "Collection must contains at least one file.";
	}

	public static class BikeType
	{
		public const string NameIsNullOrEmpty = "The provided Name cloud not be null or empty string";
		public static string NameLengthIsInvalid = "The provided Name must have length bettween {0} and {1}";
	}

	public static class Bike
	{
		public const string NameIsNullOrEmpty = "The provided Name cloud not be null or empty string";
		public static string NameLengthIsInvalid = "The provided Name must have length bettween {0} and {1}";

		public const string BikeTypeIsNullOrDefault = "The provided Bike Type cloud not be null or default";

		public static string WeightValueIsInvalid = "The provided Weight must have length bettween {0} and {1}";

		public const string BrandIsNullOrEmpty = "The provided Brand cloud not be null or empty string";
		public static string BrandLengthIsInvalid = "The provided Brand must have length bettween {0} and {1}";

		public const string ModelIsNullOrEmpty = "The provided Model cloud not be null or empty string";
		public static string ModelLengthIsInvalid = "The provided Model must have length bettween {0} and {1}";

		public static string NotesLengthIsInvalid = "When provid Notes the maximum lenght must be {0}";
		
	}
}