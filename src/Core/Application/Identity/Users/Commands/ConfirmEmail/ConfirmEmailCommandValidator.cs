// ------------------------------------------------------------------------------------------------
//  <copyright file="ConfirmEmailCommandValidator.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Commands.ConfirmEmail;

using FluentValidation;

public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        this.RuleFor(c => c.UserId)
            .NotNull()
            .NotEmpty();

        this.RuleFor(c => c.Code)
            .NotNull()
            .NotEmpty();
    }
}