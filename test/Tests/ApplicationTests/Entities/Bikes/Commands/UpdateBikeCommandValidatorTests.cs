// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateBikeCommandValidatorTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.Bikes.Commands;

using Application.Entities.Bikes.Commands.UpdateBike;

public class UpdateBikeCommandValidatorTests
{
    private readonly UpdateBikeCommandValidator _validator;
    private readonly Guid bikeId;

    public UpdateBikeCommandValidatorTests()
    {
        this._validator = new UpdateBikeCommandValidator();
        this.bikeId = Guid.NewGuid();
    }

    [Fact]
    public void UpdateBikeCommandValidator_Should_NoValidationErrorsWhenInputIsValid()
    {
        // Arrange
        var type = Guid.NewGuid();
        var user = TestsContants.UserUserId;
        var command = new UpdateBikeCommand(bikeId, "Name", type, 8.5m, "Brand", "Model", "Notes");
        
        // Act
        var errors = this._validator.Validate(command).Errors;
        
        // Assert
        Assert.True(errors.Count == 0);
    }
    
    [Theory]
    [InlineData("", "NotEmptyValidator")]
    [InlineData("aa", "MinimumLengthValidator")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "MaximumLengthValidator")]
    public void UpdateBikeCommandValidator_Should_WhenNameIsInvalid(
        string name, 
        string message)
    {
        // Arrange
        var type = Guid.NewGuid();
        var command = new UpdateBikeCommand(bikeId, name, type, 8.5m, "Brand", "Model", "Notes");

        // Act
        var errors = this._validator.Validate(command).Errors;

        // Assert
        Assert.Contains(errors, err =>
            err.PropertyName == "Name"
            && err.ErrorCode.Equals(message));
    }
    
    [Fact]
    public void UpdateBikeCommandValidator_Should_WhenBikeTypeIsInvalid()
    {
        // Arrange
        var type = Guid.Empty;
        var command = new UpdateBikeCommand(bikeId,"name", type, 8.5m, "Brand", "Model", "Notes");

        // Act
        var errors = this._validator.Validate(command).Errors;

        // Assert
        Assert.Contains(errors, err =>
            err.PropertyName == "BikeTypeId"
            && err.ErrorCode.Equals("NotEmptyValidator"));
        
        Assert.Contains(errors, err =>
            err.PropertyName == "BikeTypeId"
            && err.ErrorCode.Equals("NotEqualValidator"));
    }
    
    [Theory]
    [InlineData("-0.1", "GreaterThanOrEqualValidator")]
    [InlineData("100.0", "LessThanOrEqualValidator")]
    public void UpdateBikeCommandValidator_Should_HaveErrorWhenWeightIsInvalid(
        string number, 
        string message)
    {
        // Arrange
        var weight = Convert.ToDecimal(number);
        var type = Guid.NewGuid();
        var command = new UpdateBikeCommand(bikeId,"name", type, weight, "Brand", "Model", "Notes");

        // Act
        var errors = this._validator.Validate(command).Errors;

        // Assert
        Assert.Contains(errors, err =>
            err.PropertyName == "Weight"
            && err.ErrorCode.Equals(message));
    }
    
    [Theory]
    [InlineData("", "NotEmptyValidator")]
    [InlineData("a", "MinimumLengthValidator")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "MaximumLengthValidator")]
    public void UpdateBikeCommandValidator_Should_ReturnsErrorWhenBrandIsInvalid(
        string brand, 
        string message)
    {
        // Arrange
        var type = Guid.NewGuid();
        var command = new UpdateBikeCommand(bikeId,"name", type, 8.5m, brand, "Model", "Notes");

        // Act
        var errors = this._validator.Validate(command).Errors;

        // Assert
        Assert.Contains(errors, err =>
            err.PropertyName == "Brand"
            && err.ErrorCode.Equals(message));
    }
    
    [Theory]
    [InlineData("", "NotEmptyValidator")]
    [InlineData("a", "MinimumLengthValidator")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "MaximumLengthValidator")]
    public void UpdateBikeCommandValidator_Should_ReturnsErrorWhenModelIsInvalid(
        string model, 
        string message)
    {
        // Arrange
        var type = Guid.NewGuid();
        var command = new UpdateBikeCommand(bikeId,"name", type, 8.5m, "brand", model, "Notes");

        // Act
        var errors = this._validator.Validate(command).Errors;

        // Assert
        Assert.Contains(errors, err =>
            err.PropertyName == "Model"
            && err.ErrorCode.Equals(message));
    }
}