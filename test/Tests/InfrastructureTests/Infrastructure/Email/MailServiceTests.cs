// ------------------------------------------------------------------------------------------------
//  <copyright file="MailServiceTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.InfrastructureTests.Infrastructure.Email;

using global::Infrastructure.Email;
using Microsoft.Extensions.Options;
using Moq;

public class MailServiceTests
{
    [Fact]
        public void SendEmail_ValidData_SendsEmail()
        {
            // Arrange
            var mailOptions = new MailOptions
            {
                Mail = "your-email@example.com",
                DisplayName = "Your Name",
                Password = "your-password",
                Host = "smtp.example.com",
                Port = 587
            };
            
            var optionsMock = new Mock<IOptions<MailOptions>>();
            optionsMock.Setup(op => op.Value).Returns(mailOptions);

            var mailService = new MailService(optionsMock.Object);

            // Act
            mailService.SendEmail("recipient@example.com", "Test Subject", "<p>Test content</p>");
            
        }
}