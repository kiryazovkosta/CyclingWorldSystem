// ------------------------------------------------------------------------------------------------
//  <copyright file="SendGridEmailSender.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Services;

using Domain.Entities.Dtos;
using Interfaces;

public class SendGridEmailSender : IEmailSender
{
    public async Task SendEmailAsync(string from, string fromName, string to, string subject, string htmlContent,
        IEnumerable<EmailAttachment>? attachments = null)
    {
        throw new NotImplementedException();
    }
}