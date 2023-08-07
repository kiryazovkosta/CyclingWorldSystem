namespace Common.Constants;

public static class GlobalMessages
{
	public const string UniqueIdentifierIsNullOrDefault = "The provided Identificator cloud not be null or defaul.";
	public const string GlobalError = "An unexpected error occurred during request execution.!";
	public const string NonExistRecordError = "A record with provided Id does not exists!";
	public const string AlreadyExists = "A record with provided parameters already exists!";

	public static class Cloudinary
	{
		public const string ExceptionMessage = "Unable to upload non existsing file otr file with not valid content type";
	}

	public static class Image
	{
		public const string FileTypeIsInvalid = "The provided file does not have valid file type.";
	}

	public static class GpxFile
	{
		public const string CollectionIsEmpty = "Collection must contains at least one file.";
	}

	public static class BikeType
	{
		public const string NameIsNullOrEmpty = "The provided Name cloud not be null or empty string";
		public static readonly string NameLengthIsInvalid = "The provided Name must have length between {0} and {1}";
	}

	public static class Bike
	{
		public const string NameIsNullOrEmpty = "The provided Name cloud not be null or empty string";
		public static readonly string NameLengthIsInvalid = "The provided Name must have length between {0} and {1}";

		public const string BikeTypeIsNullOrDefault = "The provided Bike Type cloud not be null or default";

		public static readonly string WeightValueIsInvalid = "The provided Weight must have length between {0} and {1}";

		public const string BrandIsNullOrEmpty = "The provided Brand cloud not be null or empty string";
		public static readonly string BrandLengthIsInvalid = "The provided Brand must have length between {0} and {1}";

		public const string ModelIsNullOrEmpty = "The provided Model cloud not be null or empty string";
		public static readonly string ModelLengthIsInvalid = "The provided Model must have length between {0} and {1}";

		public static readonly string NotesLengthIsInvalid = "When provide Notes the maximum lenght must be {0}";
	}

	public static class User
	{
		public const string FailedToUpdate = "Failed to update user";
		public const string FailedToUpdatePassword = "Failed to change user password";
		public const string FailedToRemoveUserRoles = "Failed to remove roles of user";
		public const string FailedToAssignUserRoles = "Failed to asiign roles to user";
		public const string PasswordsAreNotEqual = "Entered passwords are not equal";
		public const string FailedToSignIn = "Failed to sign in with provided credentials";
	}
	
	public static class Role
	{
		public const string RoleHasAssociatedUsers = "The are users who are associated with this role";
	}
}