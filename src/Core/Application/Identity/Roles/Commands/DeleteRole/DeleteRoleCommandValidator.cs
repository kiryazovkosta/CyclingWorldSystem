// ------------------------------------------------------------------------------------------------
//  <copyright file="DeleteRoleCommandValidator.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Roles.Commands.DeleteRole;

using CreateRole;
using FluentValidation;

public class DeleteRoleCommandValidator
    : AbstractValidator<CreateRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        
    }
}