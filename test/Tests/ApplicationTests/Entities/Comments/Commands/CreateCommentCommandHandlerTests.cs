// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateCommentCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.Comments.Commands;

using Application.Entities.Comments.Commands.CreateComment;
using Application.Entities.Comments.Models;
using Application.Interfaces;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Mapster;
using Moq;
using Persistence;

public class CreateCommentCommandHandlerTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<IActivityRepository> activityRepository;
    private readonly Mock<ICommentRepository> commentRepository;
    private readonly Mock<ICurrentUserService> currentUserService;
    private readonly Mock<IUnitOfWork> unitOfWork;

    public CreateCommentCommandHandlerTests()
    {
        this._context = ApplicationDbContextTestFactory.Create();
        this.activityRepository = new Mock<IActivityRepository>();
        this.commentRepository = new Mock<ICommentRepository>();
        this.currentUserService = new Mock<ICurrentUserService>();
        this.unitOfWork = new Mock<IUnitOfWork>();
    }
    
    public void Dispose()
    {
        this._context.Dispose();
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenActivityRepositoryIsNull()
    {
        //Arrange & Act
        Func<CreateCommentCommandHandler> act = () => new CreateCommentCommandHandler(
            null!,
            this.commentRepository.Object,
            this.currentUserService.Object,
            this.unitOfWork.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'activityRepository')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenCommentRepositoryIsNull()
    {
        //Arrange & Act
        Func<CreateCommentCommandHandler> act = () => new CreateCommentCommandHandler(
            this.activityRepository.Object,
            null!,
            this.currentUserService.Object,
            this.unitOfWork.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'commentRepository')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenICurrentUserServiceIsNull()
    {
        //Arrange & Act
        Func<CreateCommentCommandHandler> act = () => new CreateCommentCommandHandler(
            this.activityRepository.Object,
            this.commentRepository.Object,
            null!,
            this.unitOfWork.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'currentUserService')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenUnitOfWorkIsNull()
    {
        //Arrange & Act
        Func<CreateCommentCommandHandler> act = () => new CreateCommentCommandHandler(
            this.activityRepository.Object,
            this.commentRepository.Object,
            this.currentUserService.Object,
            null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'unitOfWork')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        //Arrange
        var activityId = this._context.Set<Activity>().First().Id;
        var request = new CreateCommentRequest(activityId, "Content");
        this.activityRepository.Setup(cus => cus.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));
        var handler = new CreateCommentCommandHandler(
            this.activityRepository.Object,
            this.commentRepository.Object,
            this.currentUserService.Object,
            this.unitOfWork.Object);
        var command = request.Adapt<CreateCommentCommand>();
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.NotEqual(Guid.Empty, result.Value);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenActivityDoesNotExists()
    {
        //Arrange
        var activityId = this._context.Set<Activity>().First().Id;
        var request = new CreateCommentRequest(activityId, "Content");
        this.activityRepository.Setup(cus => cus.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(false));
        var handler = new CreateCommentCommandHandler(
            this.activityRepository.Object,
            this.commentRepository.Object,
            this.currentUserService.Object,
            this.unitOfWork.Object);
        var command = request.Adapt<CreateCommentCommand>();
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.Activity.ActivityDoesNotExists(command.ActivityId), result.Error);
    }
}
