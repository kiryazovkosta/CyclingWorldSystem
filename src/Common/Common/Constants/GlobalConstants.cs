namespace Common.Constants;

public static class GlobalConstants
{
    public static class User
    {
        public const int FirstNameMaxLength = 50;
        public const int LastNameMaxLength = 50;
    }

    public static class BikeType
    {
		public const string TableName = "BikeTypes";

		public const int NameMaxLength = 50;
		public const int NameMinLength = 3;
	}

    public static class Bike
    {
        public static string NotFoundMessage = "The bike with identifier {0} was not found.";

        public const string TableName = "Bikes";

		public const int NameMaxLength = 25;
		public const int NameMinLength = 3;

		public const int WeightPrecision = 4;
		public const int WeightScale = 2;
		public const decimal WeightMaxValue = 99.99M;
		public const decimal WeightMinValue = 0.00M;


		public const int BrandMaxLength = 50;
		public const int BrandMinLength = 2;

		public const int ModelMaxLength = 50;
		public const int ModelMinLength = 2;

		public const int NotesMaxLength = 255;

	}
}