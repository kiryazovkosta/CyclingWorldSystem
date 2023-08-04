// ------------------------------------------------------------------------------------------------
//  <copyright file="SendEmailCommand.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Emails.SendEmail;

using Abstractions.Messaging;

public sealed record SendEmailCommand(
    string From,
    string FromName,
    string To,
    string Subject,
    string HtmlContent) : ICommand;