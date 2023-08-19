namespace Application.Identity.Users.Commands.CreateUser;

using Common.Constants;
using Domain.Errors;
using FluentValidation;

public class CreateUserCommandValidator 
	: AbstractValidator<CreateUserCommand>
{
	public CreateUserCommandValidator()
	{
		this.RuleFor(user => user.UserName)
			.NotEmpty()
			.MaximumLength(GlobalConstants.User.UserNameMaxLength);

		this.RuleFor(user => user.Email)
			.NotEmpty()
			.MaximumLength(GlobalConstants.User.EmailMaxLength)
			.EmailAddress();
		
		this.RuleFor(user => user.FirstName)
			.NotEmpty()
			.MaximumLength(GlobalConstants.User.FirstNameMaxLength);
		
		this.RuleFor(user => user.LastName)
			.NotEmpty()
			.MaximumLength(GlobalConstants.User.LastNameMaxLength);

		this.RuleFor(user => user.Password)
			.Equal(user => user.ConfirmPassword)
			.NotEmpty()
			.WithMessage(DomainErrors.User.PasswordsAreNotEqual.Message);
	}
}
