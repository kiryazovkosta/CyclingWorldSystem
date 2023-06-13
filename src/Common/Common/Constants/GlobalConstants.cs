namespace Common.Constants;

public static class GlobalConstants
{
    public static class User
    {
        public const int FirstNameMaxLength = 50;
        public const int LastNameMaxLength = 50;
    }

    public static class Bike
    {
        public static string NotFoundMessage = "The bike with identifier {0} was not found.";
        public static readonly int BrandMaxLength = 50;
        public static readonly int ModelMaxLength = 50;
    }
}