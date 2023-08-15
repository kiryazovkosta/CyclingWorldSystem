// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateUserCommandValidatorTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Identity.Users.Commands;

using Application.Identity.Users.Commands.CreateUser;
using Application.Identity.Users.Models;
using Mapster;

public class CreateUserCommandValidatorTests
{
    private readonly CreateUserCommandValidator validator = new();
    
    [Fact]
    public void UpdateBikeCommandValidator_Should_NoValidationErrorsWhenInputIsValid()
    {
        // Arrange
        var type = Guid.NewGuid();
        var user = TestsContants.UserUserId;
        var request = new CreateUserRequest("UserName", "Email", "P@ssw0rd", "P@ssw0rd",
            "FirstName", "MiddleName", "LastName", null);
        var command = request.Adapt<CreateUserCommand>();
        
        // Act
        var errors = this.validator.Validate(command).Errors;
        
        // Assert
        Assert.True(errors.Count == 0);
    }
    
     [Fact]
     public void CreateUserCommandValidator_Should_HaveErrorWhenUserNameIsEmpty()
     {
         // Arrange
         var type = Guid.NewGuid();
         var user = TestsContants.UserUserId;
         var request = new CreateUserRequest("", "Email", "P@ssw0rd", "P@ssw0rd",
             "FirstName", "MiddleName", "LastName", null);
         var command = request.Adapt<CreateUserCommand>();
        
         // Act
         var errors = this.validator.Validate(command).Errors;

         // Assert
         Assert.Contains(errors, err =>
             err.PropertyName == "UserName"
             && err.ErrorCode.Equals("NotEmptyValidator"));
     }
     
     [Fact]
     public void CreateUserCommandValidator_Should_HaveErrorWhenPasswordIsEmpty()
     {
         // Arrange
         var type = Guid.NewGuid();
         var user = TestsContants.UserUserId;
         var request = new CreateUserRequest("UserName", "Email", "", "P@ssw0rd",
             "FirstName", "MiddleName", "LastName", null);
         var command = request.Adapt<CreateUserCommand>();
        
         // Act
         var errors = this.validator.Validate(command).Errors;

         // Assert
         Assert.Contains(errors, err =>
             err.PropertyName == "Password"
             && err.ErrorCode.Equals("NotEmptyValidator"));
     }
     
     [Fact]
     public void CreateUserCommandValidator_Should_HaveErrorWhenConfirmPasswordIsEmpty()
     {
         // Arrange
         var type = Guid.NewGuid();
         var user = TestsContants.UserUserId;
         var request = new CreateUserRequest("UserName", "Email", "P@ssw0rd", "",
             "FirstName", "MiddleName", "LastName", null);
         var command = request.Adapt<CreateUserCommand>();
        
         // Act
         var errors = this.validator.Validate(command).Errors;

         // Assert
         Assert.Contains(errors, err =>
             err.PropertyName == "Password"
             && err.ErrorCode.Equals("EqualValidator"));
     }
}