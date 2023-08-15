// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateActivityLikeCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.Likes.Commands;

using Application.Entities.Likes.Commands.CreateActivityLike;
using Application.Entities.Likes.Models;
using Domain.Errors;
using Domain.Identity;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Persistence;

public class CreateActivityLikeCommandHandlerTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<IActivityRepository> activityRepository;
    private readonly Mock<IActivityLikeRepository> activityLikeRepository;
    private readonly MockUserManager userManager;
    private readonly Mock<IUnitOfWork> unitOfWork;

    public CreateActivityLikeCommandHandlerTests()
    {
        this._context = ApplicationDbContextTestFactory.Create();
        this.activityRepository = new Mock<IActivityRepository>();
        this.activityLikeRepository = new Mock<IActivityLikeRepository>();
        this.userManager = new MockUserManager();
        this.unitOfWork = new Mock<IUnitOfWork>();
    }
    
    public void Dispose()
    {
        this._context.Dispose();
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<CreateActivityLikeCommandHandler> act = () => new CreateActivityLikeCommandHandler(
            null!,
            this.activityLikeRepository.Object,
            this.userManager.Object,
            this.unitOfWork.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'activityRepository')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenIActivityLikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<CreateActivityLikeCommandHandler> act = () => new CreateActivityLikeCommandHandler(
            this.activityRepository.Object,
            null!,
            this.userManager.Object,
            this.unitOfWork.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'likeRepository')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenUserManagerIsNull()
    {
        //Arrange & Act
        Func<CreateActivityLikeCommandHandler> act = () => new CreateActivityLikeCommandHandler(
            this.activityRepository.Object,
            this.activityLikeRepository.Object,
            null!,
            this.unitOfWork.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'userManager')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenUnitOfWorkIsNull()
    {
        //Arrange & Act
        Func<CreateActivityLikeCommandHandler> act = () => new CreateActivityLikeCommandHandler(
            this.activityRepository.Object,
            this.activityLikeRepository.Object,
            this.userManager.Object,
            null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'unitOfWork')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenActivityDoesNotExists()
    {
        //Arrange
        this.activityRepository.Setup(ar => ar.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(false));
        var activityId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var handler = new CreateActivityLikeCommandHandler(
            this.activityRepository.Object,
            this.activityLikeRepository.Object,
            this.userManager.Object,
            this.unitOfWork.Object);
        var command = new CreateActivityLikeCommand(activityId, userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.Activity.ActivityDoesNotExists(command.ActivityId), result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenUserDoesNotExists()
    {
        //Arrange
        this.activityRepository.Setup(ar => ar.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(null));
        var activityId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var handler = new CreateActivityLikeCommandHandler(
            this.activityRepository.Object,
            this.activityLikeRepository.Object,
            this.userManager.Object,
            this.unitOfWork.Object);
        var command = new CreateActivityLikeCommand(activityId, userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.User.NonExistsUser, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccessWhenInputIsValid()
    {
        //Arrange
        this.activityRepository.Setup(ar => ar.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));
        this.userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(new User()));
        var activityId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var handler = new CreateActivityLikeCommandHandler(
            this.activityRepository.Object,
            this.activityLikeRepository.Object,
            this.userManager.Object,
            this.unitOfWork.Object);
        var request = new CreateActivityLikeRequest(activityId, userId);
        var command = request.Adapt<CreateActivityLikeCommand>();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.True(result.Value);
    }
}