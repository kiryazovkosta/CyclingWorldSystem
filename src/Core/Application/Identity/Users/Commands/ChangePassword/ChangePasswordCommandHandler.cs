// ------------------------------------------------------------------------------------------------
//  <copyright file="ChangePasswordCommandHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Commands.ChangePassword;

using Abstractions.Messaging;
using Domain.Errors;
using Domain.Identity;
using Domain.Shared;
using Microsoft.AspNetCore.Identity;

public class ChangePasswordCommandHandler
    : ICommandHandler<ChangePasswordCommand, bool>
{
    private readonly UserManager<User> _userManager;

    public ChangePasswordCommandHandler(UserManager<User> userManager)
    {
        this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user is null)
        {
            return Result.Failure<bool>(DomainErrors.User.NonExistsUser);
        }

        if (request.NewPassword != request.ConfirmNewPassword)
        {
            return Result.Failure<bool>(DomainErrors.User.PasswordsAreNotEqual);
        }

        var signInResult = await this._userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        return !signInResult.Succeeded ? 
            Result.Failure<bool>(DomainErrors.User.FailedToSignIn) 
            : Result.Success<bool>(true);
    }
}