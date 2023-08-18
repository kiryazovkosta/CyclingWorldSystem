// ------------------------------------------------------------------------------------------------
//  <copyright file="ConfirmEmailCommandValidatorTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Users.Commands;

using Application.Identity.Users.Commands.ConfirmEmail;
using Mapster;

public class ConfirmEmailCommandValidatorTests
{
    private readonly ConfirmEmailCommandValidator validator = new();
    
    [Fact]
    public void ConfirmEmailCommandValidator_Should_NoValidationErrorsWhenInputIsValid()
    {
        // Arrange
        var type = Guid.NewGuid();
        var user = TestsContants.UserUserId;
        var request = new ConfirmEmailCommand("12345678", "1a2s3d4f");
        var command = request.Adapt<ConfirmEmailCommand>();
        
        // Act
        var errors = this.validator.Validate(command).Errors;
        
        // Assert
        Assert.True(errors.Count == 0);
    }
    
    [Fact]
    public void ConfirmEmailCommandValidator_Should_HaveErrorWhenUserNameIsEmpty()
    {
        // Arrange
        var type = Guid.NewGuid();
        var user = TestsContants.UserUserId;
        var request = new ConfirmEmailCommand("", "1a2s3d4f");
        var command = request.Adapt<ConfirmEmailCommand>();
        
        // Act
        var errors = this.validator.Validate(command).Errors;

        // Assert
        Assert.Contains(errors, err =>
            err.PropertyName == "UserId"
            && err.ErrorCode.Equals("NotEmptyValidator"));
    }
    
    [Fact]
    public void ConfirmEmailCommandValidator_Should_HaveErrorWhenCodeIsEmpty()
    {
        // Arrange
        var type = Guid.NewGuid();
        var user = TestsContants.UserUserId;
        var request = new ConfirmEmailCommand("12345678", "");
        var command = request.Adapt<ConfirmEmailCommand>();
        
        // Act
        var errors = this.validator.Validate(command).Errors;

        // Assert
        Assert.Contains(errors, err =>
            err.PropertyName == "Code"
            && err.ErrorCode.Equals("NotEmptyValidator"));
    }
}