// ------------------------------------------------------------------------------------------------
//  <copyright file="MailService.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Infrastructure.Email;

using System.Net.Mail;
using Application.Interfaces;
using Domain.Entities.Dtos;
using Microsoft.Extensions.Options;

public class MailService : IEmailSender
{
    private readonly MailOptions _mailOptions;

    public MailService(IOptions<MailOptions> mailOptions)
    {
        this._mailOptions = mailOptions.Value ?? throw new ArgumentNullException(nameof(mailOptions));
    }

    public Task SendEmailAsync(string to, string subject, string htmlContent)
    {
        return Task.Run(() => SendEmail(to, subject, htmlContent));
    }

    public void SendEmail(string to, string subject, string htmlContent)
    {
        try
        {
            using var mailMessage = new MailMessage(this._mailOptions.Mail, to, subject, htmlContent);
            using var client = new SmtpClient(this._mailOptions.Host, this._mailOptions.Port) {UseDefaultCredentials = true};
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = mailMessage.Body.Replace("\r\n", "<br />").Replace("\n", "<br />");
            mailMessage.Body = mailMessage.Body.Replace("\t", "&nbsp;&nbsp;&nbsp;");

            client.Send(mailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}