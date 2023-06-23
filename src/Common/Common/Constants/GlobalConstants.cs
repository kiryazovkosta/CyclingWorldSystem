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

	public static class Image
	{
		public const int UrlMaxLength = 2048;
	}

	public static class TrainingPlan
	{
		public const string TableName = "TrainingPlans";

		public const int TitleMaxLength = 80;
		public const int TitleMinLength = 5;
	}

	public static class UserChallenge
	{
		public const string TableName = "UsersChallenges";
	}

	public static class UserTrainingPlan
	{
		public const string TableName = "UsersTrainingPlans";
	}

	public static class Waypoint
	{
		public const string TableName = "Waypoints";

		public const int LatitudePrecision = 12;
		public const int LatitudeScale = 9;

		public const int LongitudePrecision = 12;
		public const int LongitudeScale = 9;

		public const int SpeedPrecision = 12;
		public const int SpeedScale = 3;
	}

	public static class Workout
	{
		public const string TableName = "Workout";

		public const int TitleMaxLength = 80;
		public const int TitleMinLength = 3;
	}
}