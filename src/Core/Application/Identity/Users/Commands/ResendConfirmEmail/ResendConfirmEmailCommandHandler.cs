// ------------------------------------------------------------------------------------------------
//  <copyright file="ResendConfirmEmailCommandHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Commands.ResendConfirmEmail;

using System.Text;
using System.Text.Encodings.Web;
using Abstractions.Messaging;
using Domain.Errors;
using Domain.Identity;
using Domain.Repositories.Abstractions;
using Domain.Shared;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

public class ResendConfirmEmailCommandHandler
    : ICommandHandler<ResendConfirmEmailCommand, bool>
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailSender _emailSender;
    private readonly IUnitOfWork _context;


    public ResendConfirmEmailCommandHandler(
        UserManager<User> userManager, 
        IEmailSender emailSender, 
        IUnitOfWork context)
    {
        this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this._emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        this._context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Result<bool>> Handle(ResendConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await this._userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result.Failure<bool>(DomainErrors.User.NonExistsUser);
        }

        if (user.EmailConfirmed)
        {
            return Result.Failure<bool>(DomainErrors.User.NonExistsUser);
        }

        var userId = await this._userManager.GetUserIdAsync(user);
        userId = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(userId));
        var code = await this._userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var confirmUrl = $"http://localhost:5268/Account/ConfirmEmail?code={code}&userId={userId}";
        await this._emailSender.SendEmailAsync(
            request.Email, 
            "Confirm your email", 
            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmUrl)}'>clicking here</a>.");

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success<bool>(true);
    }
}