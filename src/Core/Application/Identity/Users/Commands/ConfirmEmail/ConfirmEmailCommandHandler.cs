﻿// ------------------------------------------------------------------------------------------------
//  <copyright file="ConfirmEmailCommandHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Commands.ConfirmEmail;

using System.Text;
using Abstractions.Messaging;
using Domain.Errors;
using Domain.Identity;
using Domain.Repositories.Abstractions;
using Domain.Shared;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

public class ConfirmEmailCommandHandler
    : ICommandHandler<ConfirmEmailCommand, bool>
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailSender _emailSender;
    private readonly IUnitOfWork _context;

    public ConfirmEmailCommandHandler(
        UserManager<User> userManager, 
        IEmailSender emailSender, 
        IUnitOfWork context)
    {
        this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this._emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        this._context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Result<bool>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var userId = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.UserId));
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure<bool>(DomainErrors.User.NonExistsUser);
        }

        var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (!result.Succeeded)
        {
            return Result.Failure<bool>(DomainErrors.AnUnexpectedError(nameof(ConfirmEmailCommand)));
        }

        return Result.Success<bool>(true);
    }
}