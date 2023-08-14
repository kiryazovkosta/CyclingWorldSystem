// ------------------------------------------------------------------------------------------------
//  <copyright file="FromFileExtensionsTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.InfrastructureTests.Infrastructure.Extensions;

using global::Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;

public class FromFileExtensionsTests
{
    [Fact]
    public async Task ReadAsStringAsync_ValidFile_ReturnsFileContent()
    {
        // Arrange
        var content = "Hello, world!";
        var contentBytes = System.Text.Encoding.UTF8.GetBytes(content);
        var formFile = new FormFile(new MemoryStream(contentBytes), 0, contentBytes.Length, "file", "file.txt");

        // Act
        var result = await formFile.ReadAsStringAsync();

        // Assert
        Assert.Equal(content, result);
    }

    [Fact]
    public async Task ReadAsStringAsync_EmptyFile_ReturnsEmptyString()
    {
        // Arrange
        var contentBytes = Array.Empty<byte>();
        var formFile = new FormFile(new MemoryStream(contentBytes), 0, contentBytes.Length, "file", "file.txt");

        // Act
        var result = await formFile.ReadAsStringAsync();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public async Task ReadAsStringAsync_NullFile_ThrowsArgumentNullException()
    {
        // Arrange
        IFormFile formFile = null!;

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await formFile.ReadAsStringAsync());
    }
}