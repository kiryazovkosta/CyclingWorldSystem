using Common.Constants;
using Domain.Shared;

namespace Domain.Errors;

public static class DomainErrors
{
	public static Error DeleteOperationFailed(Guid id, string command) => new(
		command,
		$"There is a error with deleting entity with Id {id}");

	public static Error UnauthorizedAccess(string command) => new(
		command,
		"You does not have permissions to access this resource");

	public static Error AnUnexpectedError(string command) => new(
		command,
		GlobalMessages.GlobalError);

	public static class User
	{
		public static Error LogInFailed => new(
			"LogInUserCommand.LogIn",
			GlobalConstants.User.FailedToLogInMessage);

		public static Error EmailIsNotConfirmed => new(
			"LogInUserCommand",
			GlobalConstants.User.EmailIsNotConfirmed);

		public static Error EmailIsAlreadyConfirmed => new(
			"LogInUserCommand",
			GlobalConstants.User.EmailIsAlredyNotConfirmed);
		
		public static Error NonExistsUser => new(
			"EditUserCommand.UserId",
			GlobalMessages.NonExistRecordError);
		
		public static Error FailedToUpdateUser => new(
			"EditUserCommand.UserId",
			GlobalMessages.User.FailedToUpdate);
		
		public static Error FailedToUpdatePassword => new(
			"EditUserCommand.UserId",
			GlobalMessages.User.FailedToUpdatePassword);
		
		public static Error FailedToRemoveUserRoles => new(
			"UpdateUserRolesCommand.UserId",
			GlobalMessages.User.FailedToRemoveUserRoles);
		
		public static Error FailedToAssignUserRoles => new(
			"UpdateUserRolesCommand.UserId",
			GlobalMessages.User.FailedToAssignUserRoles);

		public static Error PasswordsAreNotEqual => new(
			"User.ResetPassword.Password",
			GlobalMessages.User.PasswordsAreNotEqual);
		
	}
	
	public static class Role
	{
		public static Error NonExistsRole => new(
			"GetRoleByIdQuery.RoleId",
			GlobalMessages.NonExistRecordError);
		
		public static Error RoleAlreadyExists => new(
			"CreateRoleCommand.RoleName",
			GlobalMessages.AlreadyExists);
		
		public static Error FailedToCreate => new(
			"CreateRoleCommand.RoleName",
			GlobalMessages.GlobalError);

		public static Error NonEmptyRole => new(
			"DeleteRoleCommand.RoleId",
			GlobalMessages.Role.RoleHasAssociatedUsers);
	}

	public static class Image
	{
		public static Error InvalidFileType => new(
			"Image.Create.File",
			GlobalMessages.Image.FileTypeIsInvalid);

    }


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
		public static Error NameIsNullOrEmpty => new(
			"Bike.Create.Name",
			GlobalMessages.Bike.NameIsNullOrEmpty);

		public static Error NameLengthIsInvalid(int min, int max) => new(
			"Bike.Create.Name",
			string.Format(GlobalMessages.Bike.NameLengthIsInvalid, min, max));

		public static Error BikeTypeIsInvalid => new(
			"Bike.Create.BikeType",
			GlobalMessages.Bike.BikeTypeIsNullOrDefault);

		public static Error WeightIsInvalid(decimal min, decimal max) => new(
			"Bike.Create.Weight",
			string.Format(GlobalMessages.Bike.WeightValueIsInvalid, min, max));

		public static Error BrandIsNullOrEmpty => new(
			"Bike.Create.Brand",
			GlobalMessages.Bike.BrandIsNullOrEmpty);

		public static Error BrandLengthIsInvalid(int min, int max) => new(
			"Bike.Create.Brand",
			string.Format(GlobalMessages.Bike.BrandLengthIsInvalid, min, max));

		public static Error ModelIsNullOrEmpty => new(
			"Bike.Create.Model",
			GlobalMessages.Bike.ModelIsNullOrEmpty);

		public static Error ModelLengthIsInvalid(int min, int max) => new(
			"Bike.Create.Brand",
			string.Format(GlobalMessages.Bike.BrandLengthIsInvalid, min, max));

		public static Error NotesLengthIsInvalid(int max) => new(
			"Bike.Create.Brand",
			string.Format(GlobalMessages.Bike.NotesLengthIsInvalid, max));

		public static Error BikeDoesNotExists(Guid id) => new(
			"Bike.Id",
			$"The bike with provided Id {id} does not exists.");
	}

	public static class Activity
	{
		public static Error ActivityDoesNotExists(Guid id) => new(
			"Activity.Id",
			$"The activity with provided Id {id} does not exists.");
	}

	public static class ActivityLike
	{
		public static Error LikeDoesNotExists(Guid id) => new(
			"ActivityLike.GetById.Id",
			$"The like for activity with provided Id {id} and user does not exists.");
	}

	public static class Comments 
	{
		public static Error ContentIsEmptyOrNull => new(
			"Comments.Create.Content",
				"The content can not be null or empty string");

		public static Error ContentInvalidLength => new(
			"Comments.Create.Content",
			$"The content length must be between {GlobalConstants.Comment.ContentMinLength} and {GlobalConstants.Comment.ContentMaxLength} symbols");

		public static Error CommentDoesNotExists(Guid id) => new(
			"Comments.Create.Content",
			$"The comment with provided Id {id} does not exists");
	} 

}
