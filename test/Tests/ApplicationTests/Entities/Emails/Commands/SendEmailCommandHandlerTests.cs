// ------------------------------------------------------------------------------------------------
//  <copyright file="SendEmailCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.Emails.Commands;

using Application.Entities.Emails.SendEmail;
using Application.Interfaces;
using Infrastructure.Email;
using Moq;

public class SendEmailCommandHandlerTests
{
    private readonly Mock<IEmailSender> emailSender;

    public SendEmailCommandHandlerTests()
    {
        this.emailSender = new Mock<IEmailSender>();
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenIEmailSenderIsNull()
    {
        //Arrange & Act
        Func<SendEmailCommandHandler> act = () => new SendEmailCommandHandler(null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'emailSender')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnsSuccess()
    {
        //Arrange & Act
        var handler = new SendEmailCommandHandler(this.emailSender.Object);
        var command = new SendEmailCommand("from", "fromName", "to", "subject", "content");
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }
}