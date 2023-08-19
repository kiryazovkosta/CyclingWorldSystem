// ------------------------------------------------------------------------------------------------
//  <copyright file="UpdateRoleCommandValidator.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Roles.Commands.UpdateRole;

using FluentValidation;

public class UpdateRoleCommandValidator
    : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        this.RuleFor(c => c.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(256);
    }
}