// ------------------------------------------------------------------------------------------------
//  <copyright file="CloudinaryUploadExceptionTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Exceptions;

using Application.Exceptions;

public class CloudinaryUploadExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetErrorMessage()
    {
        // Arrange
        var errorMessage = "Cloudinary upload failed";

        // Act
        var cloudinaryException = new CloudinaryUploadException(errorMessage);

        // Assert
        Assert.Equal(errorMessage, cloudinaryException.Message);
    }
}