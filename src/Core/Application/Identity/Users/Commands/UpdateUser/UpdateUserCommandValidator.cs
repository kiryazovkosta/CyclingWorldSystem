// ------------------------------------------------------------------------------------------------
//  <copyright file="UpdateUserCommandValidator.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Commands.UpdateUser;

using Common.Constants;
using FluentValidation;

public class UpdateUserCommandValidator
 : AbstractValidator<UpdateUserCommand>
{
	public UpdateUserCommandValidator()
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
	}
}