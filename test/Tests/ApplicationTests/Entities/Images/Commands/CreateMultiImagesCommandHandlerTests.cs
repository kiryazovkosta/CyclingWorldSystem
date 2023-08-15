// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateMultiImagesCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.Images.Commands;

using Application.Entities.Images.Commands.CreateMultiImages;
using Application.Interfaces;
using Domain.Errors;
using Microsoft.AspNetCore.Http;
using Moq;

public class CreateMultiImagesCommandHandlerTests
{
    private readonly Mock<ICloudinaryService> cloudinaryService = new();
    private readonly Mock<IFormFile> fromFile = new();
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<CreateMultiImagesCommandHandler> act = () => new CreateMultiImagesCommandHandler(null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'cloudinaryService')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenFileIsNotValid()
    {
        //Arrange
        this.cloudinaryService.Setup(cs => cs.IsFileValid(It.IsAny<IFormFile>()))
            .Returns(false);
        var handler = new CreateMultiImagesCommandHandler(this.cloudinaryService.Object);
        var command = new CreateMultiImagesCommand(new List<IFormFile>() { this.fromFile.Object });
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.Image.InvalidFileType, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccessWhenFileIsNotValid()
    {
        //Arrange
        this.cloudinaryService.Setup(cs => cs.IsFileValid(It.IsAny<IFormFile>()))
            .Returns(true);
        this.cloudinaryService.Setup(cs => cs.UploadMultiAsync(It.IsAny<List<IFormFile>>()))
            .Returns(Task.FromResult(new List<string>() { "url1", "url2" }));
        var handler = new CreateMultiImagesCommandHandler(this.cloudinaryService.Object);
        var command = new CreateMultiImagesCommand(new List<IFormFile>() { this.fromFile.Object, this.fromFile.Object });
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(2, result.Value.Count);
    }
}