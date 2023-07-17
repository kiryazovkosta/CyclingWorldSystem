namespace Application.Identity.Users.Commands.CreateUser;

using FluentValidation;

public class CreateUserCommandValidator 
	: AbstractValidator<CreateUserCommand>
{
	public CreateUserCommandValidator()
	{
		this.RuleFor(user => user.UserName).NotEmpty();

		this.RuleFor(user => user.Password)
			.Equal(user => user.ConfirmPassword)
			.NotEmpty();
	}
}
