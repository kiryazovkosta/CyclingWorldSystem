// ------------------------------------------------------------------------------------------------
//  <copyright file="ValidationExceptionTestsTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Exceptions;

using Application.Exceptions;

public class ValidationExceptionTestsTests
{
    [Fact]
    public void Constructor_ShouldInitializeErrors()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "Field1", new[] { "Error1", "Error2" } },
            { "Field2", new[] { "Error3" } }
        };

        // Act
        var validationException = new ValidationException(errors);

        // Assert
        Assert.NotNull(validationException.Errors);
        Assert.Equal(errors, validationException.Errors);
    }

    [Fact]
    public void Constructor_ShouldSetErrorMessage()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "Field1", new[] { "Error1", "Error2" } }
        };

        // Act
        var validationException = new ValidationException(errors);

        // Assert
        Assert.Equal("Validation errors occurred", validationException.Message);
    }
}