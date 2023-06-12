namespace Application.Identity.Users.Commands.CreateUser;

using FluentValidation;

public class CreateUserCommandValidator 
	: AbstractValidator<CreateUserCommand>
{
	public CreateUserCommandValidator()
	{
		this.RuleFor(user => user.UserName).NotEmpty();
	}
}
