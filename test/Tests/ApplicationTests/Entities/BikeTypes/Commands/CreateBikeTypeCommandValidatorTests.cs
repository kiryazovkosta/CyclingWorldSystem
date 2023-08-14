// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateBikeTypeCommandValidatorTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.BikeTypes.Commands;

using Application.Entities.BikeTypes.Commands.CreateBikeType;

public class CreateBikeTypeCommandValidatorTests
{
    private readonly CreateBikeTypeCommandValidator _validator;

    public CreateBikeTypeCommandValidatorTests()
    {
        this._validator = new CreateBikeTypeCommandValidator();
    }
    
    [Fact]
    public void CreateBikeTypeCommandValidator_Should_NoValidationErrorsWhenInputIsValid()
    {
        // Arrange
        var type = Guid.NewGuid();
        var user = TestsContants.UserUserId;
        var command = new CreateBikeTypeCommand("Name");
        
        // Act
        var errors = this._validator.Validate(command).Errors;
        
        // Assert
        Assert.True(errors.Count == 0);
    }
    
    [Theory]
    [InlineData("", "NotEmptyValidator")]
    [InlineData("aa", "MinimumLengthValidator")]
    [InlineData("aaaaaaaaa1aaaaaaaaa2aaaaaaaaa3aaaaaaaaa4aaaaaaaaa5a", "MaximumLengthValidator")]
    public void CreateBikeCommandValidator_Should_WhenNameIsInvalid(
        string name, 
        string message)
    {
        // Arrange
        var type = Guid.NewGuid();
        var command = new CreateBikeTypeCommand(name);

        // Act
        var errors = this._validator.Validate(command).Errors;

        // Assert
        Assert.Contains(errors, err =>
            err.PropertyName == "Name"
            && err.ErrorCode.Equals(message));
    }
}