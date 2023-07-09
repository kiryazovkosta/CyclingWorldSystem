namespace Common.Constants;

public static class GlobalConstants
{
    public static class User
    {
        public const int FirstNameMaxLength = 50;
        public const int LastNameMaxLength = 50;

		public const string AdministratorUserName = "Admin";
		public const string ManagerUserName = "Manager";
		public const string UserUserName = "User";

		public const string DefaultPassword = "P@ssw0rd@a!";
		public const string DefaultEmailDomain = "@example.com";

		public static string NotFoundMessage = "The provided userName {0} does not exists.";
	}

	public static class Role
	{
		public const string AdministratorRoleName = "Administrator";
		public const string ManagerRoleName = "Manager";
		public const string UserRoleName = "User";
	}

	public static class Activity
	{
		public const string TableName = "Activities";

		public const int TitleMaxLength = 80;
		public const int TitleMinLength = 5;

		public const int DescriptionMaxLength = 255;
		public const int DescriptionMinLength = 10;

		public const int PrivateNotesMaxLength = 255;
		public const int PrivateNotesMinLength = 10;

		public const int DestancePrecision = 7;
		public const int DestanceScale = 3;
	}

	public static class ActivityLike
	{
		public const string TableName = "Likes";
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

	public static class Challenge
	{
		public const string TableName = "Challenges";

		public const int TitleMaxLength = 80;
		public const int TitleMinLength = 5;

		public const int DescriptionMaxLength = 255;
		public const int DescriptionMinLength = 10;
	}

	public static class Comment
	{
		public const string TableName = "Comments";

		public const int ContentMaxLength = 255;
		public const int ContentMinLength = 5;
	}

	public static class Image
	{
		public const string TableName = "Images";

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

	public static class GpXFile
	{
		public const string XmlNamespacePattern = 
			"\\s+xmlns\\s*(:\\w)?\\s*=\\s*\\\"(?<url>[^\\\"]*)\\\"";
	}
}