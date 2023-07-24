// ------------------------------------------------------------------------------------------------
//  <copyright file="DeleteUserCommandValidator.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Commands.DeleteUser;

using FluentValidation;

public class DeleteUserCommandValidator 
    : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        
    }
}