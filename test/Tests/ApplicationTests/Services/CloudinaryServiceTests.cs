// ------------------------------------------------------------------------------------------------
//  <copyright file="CloudinaryServiceTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Services;

using Application.Exceptions;
using Application.Interfaces;
using Application.Services;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Moq;

public class CloudinaryServiceTests
{
    [Fact]
    public void IsFileValid_ValidFile_ReturnsTrue()
    {
        // Arrange
        var cloudinary = new MockedCloudinary();
        var service = new CloudinaryService(cloudinary);

        var formFile = new Mock<IFormFile>();
        formFile.Setup(file => file.ContentType).Returns("image/jpeg");

        // Act
        var result = service.IsFileValid(formFile.Object);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsFileValid_InvalidFile_ReturnsFalse()
    {
        // Arrange
        var cloudinary = new MockedCloudinary();
        var service = new CloudinaryService(cloudinary);

        var formFile = new Mock<IFormFile>();
        formFile.Setup(file => file.ContentType).Returns("application/pdf");

        // Act
        var result = service.IsFileValid(formFile.Object);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsFileValid_NullFile_ReturnsFalse()
    {
        // Arrange
        var cloudinary = new MockedCloudinary();
        var service = new CloudinaryService(cloudinary);

        // Act
        var result = service.IsFileValid(null);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public async Task UploadAsync_ValidFile_ReturnsUrl()
    {
        // Arrange
        var cloudinaryMock = new MockedCloudinary();
        var cloudinaryService = new CloudinaryService(cloudinaryMock);

        var formFileMock = new Mock<IFormFile>();
        var fileBytes = new byte[] { 0x00, 0x01, 0x02 }; // Sample file bytes
        formFileMock.Setup(f => f.FileName).Returns("sample.jpg");
        formFileMock.Setup(f => f.ContentType).Returns("image/jpg");
        formFileMock.Setup(f => f.Length).Returns(fileBytes.Length);
        formFileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(fileBytes));

        // Act
        var url = await cloudinaryService.UploadAsync(formFileMock.Object);

        // Assert
        Assert.NotNull(url);
        // Add more assertions as needed
    }

    [Fact]
    public async Task UploadAsync_InvalidFile_ThrowsCloudinaryUploadException()
    {
        // Arrange
        var cloudinaryMock = new MockedCloudinary();
        var cloudinaryService = new CloudinaryService(cloudinaryMock);

        var formFileMock = new Mock<IFormFile>();
        formFileMock.Setup(f => f.FileName).Returns("invalid.txt");
        formFileMock.Setup(f => f.Length).Returns(10); // Invalid file length
        formFileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(new byte[10]));

        // Act & Assert
        await Assert.ThrowsAsync<CloudinaryUploadException>(() => cloudinaryService.UploadAsync(formFileMock.Object));
    }

    [Fact]
    public async Task UploadMultiAsync_ValidFiles_ReturnsUrls()
    {
        // Arrange
        var cloudinaryMock = new MockedCloudinary();
        var cloudinaryService = new CloudinaryService(cloudinaryMock);

        var formFiles = new List<IFormFile>
        {
            // Create and setup IFormFile instances as needed
        };

        // Act
        var urls = await cloudinaryService.UploadMultiAsync(formFiles);

        // Assert
        Assert.NotNull(urls);
        Assert.Equal(formFiles.Count, urls.Count);
        // Add more assertions as needed
    }
}