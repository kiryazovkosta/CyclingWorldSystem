// ------------------------------------------------------------------------------------------------
//  <copyright file="IEmailSender.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Interfaces;

using Domain.Entities.Dtos;

public interface IEmailSender
{
    Task SendEmailAsync(
        string to,
        string subject,
        string content);

    void SendEmail(
        string to,
        string subject,
        string content);
}