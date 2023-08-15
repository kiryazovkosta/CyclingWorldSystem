// ------------------------------------------------------------------------------------------------
//  <copyright file="SendEmailCommandHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Emails.SendEmail;

using Abstractions.Messaging;
using Domain.Shared;
using Interfaces;

public class SendEmailCommandHandler
    : ICommandHandler<SendEmailCommand>
{
    private readonly IEmailSender _emailSender;

    public SendEmailCommandHandler(IEmailSender emailSender)
    {
        this._emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
    }

    public async Task<Result> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        await this._emailSender.SendEmailAsync(
            "kosta.lkiryazov@gmail.com", 
        "Send Email", "Send a testing email");

        return Result.Success();
    }
}