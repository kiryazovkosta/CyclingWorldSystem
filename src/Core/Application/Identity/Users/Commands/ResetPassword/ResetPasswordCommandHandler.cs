﻿// ------------------------------------------------------------------------------------------------
//  <copyright file="ResetPasswordCommandHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Commands.ResetPassword;

using System.Text;
using Abstractions.Messaging;
using Domain.Errors;
using Domain.Identity;
using Domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

public class ResetPasswordCommandHandler
    : ICommandHandler<ResetPasswordCommand, bool>
{
    private readonly UserManager<User> _userManager;

    public ResetPasswordCommandHandler(UserManager<User> userManager)
    {
        this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<Result<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.UserId));
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure<bool>(DomainErrors.User.NonExistsUser);
        }

        if (request.Password != request.ConfirmPassword)
        {
            return Result.Failure<bool>(DomainErrors.User.PasswordsAreNotEqual);
        }

        var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
        var result = await _userManager.ResetPasswordAsync(user, code, request.Password);
        if (!result.Succeeded)
        {
            return Result.Failure<bool>(DomainErrors.AnUnexpectedError(nameof(ResetPasswordCommand)));
        }
        
        return Result.Success<bool>(true);
    }
}