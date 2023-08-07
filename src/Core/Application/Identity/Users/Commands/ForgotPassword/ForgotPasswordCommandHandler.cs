// ------------------------------------------------------------------------------------------------
//  <copyright file="ForgotPasswordCommandHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Commands.ForgotPassword;

using System.Text;
using System.Text.Encodings.Web;
using Abstractions.Messaging;
using Domain.Errors;
using Domain.Identity;
using Domain.Shared;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

public class ForgotPasswordCommandHandler
    : ICommandHandler<ForgotPasswordCommand, bool>
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailSender _emailSender;

    public ForgotPasswordCommandHandler(UserManager<User> userManager, IEmailSender emailSender)
    {
        this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this._emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
    }

    public async Task<Result<bool>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
        {
            return Result.Failure<bool>(DomainErrors.User.NonExistsUser);
        }
        
        var userId = await this._userManager.GetUserIdAsync(user);
        userId = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(userId));
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var resetUrl = $"http://localhost:5268/Account/ResetPassword?userId={userId}&code={code}";
        await this._emailSender.SendEmailAsync(
            user.Email!, 
            "Reset password", 
            $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(resetUrl)}'>clicking here</a>.");

        return Result.Success<bool>(true);
    }
}