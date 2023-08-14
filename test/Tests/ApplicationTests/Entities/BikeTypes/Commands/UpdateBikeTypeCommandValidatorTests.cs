// ------------------------------------------------------------------------------------------------
//  <copyright file="UpdateBikeTypeCommandValidatorTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.BikeTypes.Commands;

using Application.Entities.BikeTypes.Commands.CreateBikeType;
using Application.Entities.BikeTypes.Commands.UpdateBikeType;

public class UpdateBikeTypeCommandValidatorTests
{
    private readonly UpdateBikeTypeCommandValidator _validator;

    public UpdateBikeTypeCommandValidatorTests()
    {
        this._validator = new UpdateBikeTypeCommandValidator();
    }
    
    [Fact]
    public void UpdateBikeTypeCommandValidatorTests_Should_NoValidationErrorsWhenInputIsValid()
    {
        // Arrange
        var type = Guid.NewGuid();
        var user = TestsContants.UserUserId;
        var command = new UpdateBikeTypeCommand(Guid.NewGuid(), "New Name");
        
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
        var command = new UpdateBikeTypeCommand(Guid.NewGuid(), name);
    
        // Act
        var errors = this._validator.Validate(command).Errors;
    
        // Assert
        Assert.Contains(errors, err =>
            err.PropertyName == "Name"
            && err.ErrorCode.Equals(message));
    }
    
    [Fact]
    public void CreateBikeCommandValidator_Should_WhenBukeTypeIdInvalid()
    {
        // Arrange
        var command = new UpdateBikeTypeCommand(Guid.Empty, "NewName");
    
        // Act
        var errors = this._validator.Validate(command).Errors;
    
        // Assert
        Assert.Contains(errors, err =>
            err.PropertyName == "Id"
            && err.ErrorCode.Equals("NotEmptyValidator"));
    }
}