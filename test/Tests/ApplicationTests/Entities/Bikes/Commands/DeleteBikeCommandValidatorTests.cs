// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateBikeCommandValidatorTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.Bikes.Commands;

using Application.Entities.Bikes.Commands.DeleteBike;

public class DeleteBikeCommandValidatorTests
{
    private readonly DeleteBikeCommandValidator _validator;

    public DeleteBikeCommandValidatorTests()
    {
        this._validator = new DeleteBikeCommandValidator();
    }

    [Fact]
    public void UpdateBikeCommandValidator_Should_NoValidationErrorsWhenInputIsValid()
    {
        // Arrange
        var type = Guid.NewGuid();
        var user = TestsContants.UserUserId;
        var command = new DeleteBikeCommand(Guid.NewGuid());
        
        // Act
        var errors = this._validator.Validate(command).Errors;
        
        // Assert
        Assert.True(errors.Count == 0);
    }
}