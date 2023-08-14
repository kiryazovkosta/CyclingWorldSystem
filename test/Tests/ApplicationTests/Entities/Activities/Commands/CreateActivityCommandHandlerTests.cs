// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateActivityCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.Activities.Commands;

using Application.Entities.Activities.Commands.CreateActivity;
using Application.Entities.Activities.Models;
using Application.Interfaces;
using Common.Enumerations;
using Domain.Errors;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using FluentAssertions;
using Mapster;
using Moq;
using Persistence;

public class CreateActivityCommandHandlerTests: IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<IActivityRepository> activityRepository;
    private readonly Mock<IWaypointRepository> waypointRepository;
    private readonly Mock<ICurrentUserService> currentUserService;
    private readonly Mock<IUnitOfWork> unitOfWork;

    public CreateActivityCommandHandlerTests()
    {
        this._context = ApplicationDbContextTestFactory.Create();
        this.activityRepository = new Mock<IActivityRepository>();
        this.waypointRepository = new Mock<IWaypointRepository>();
        this.currentUserService = new Mock<ICurrentUserService>();
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
        Func<CreateActivityCommandHandler> act = () => new CreateActivityCommandHandler(
            null!,
            this.waypointRepository.Object,
            this.currentUserService.Object,
            this.unitOfWork.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'activitiesRepository')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenCurrentUserServiceIsNull()
    {
        //Arrange & Act
        Func<CreateActivityCommandHandler> act = () => new CreateActivityCommandHandler(
            this.activityRepository.Object,
            null!,
            this.currentUserService.Object,
            this.unitOfWork.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'waypointsRepository')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenICurrentUserServiceIsNull()
    {
        //Arrange & Act
        Func<CreateActivityCommandHandler> act = () => new CreateActivityCommandHandler(
            this.activityRepository.Object,
            this.waypointRepository.Object,
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
        Func<CreateActivityCommandHandler> act = () => new CreateActivityCommandHandler(
            this.activityRepository.Object,
            this.waypointRepository.Object,
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
        var request = new CreateActivityRequest("title", "Description", null, 10m, new TimeSpan(10),
            null, null, VisibilityLevelType.All, DateTime.Now, Guid.NewGuid(),
            new List<string>() { "img1.png" }, Guid.NewGuid());
        this.currentUserService.Setup(cus => cus.GetCurrentUserId())
            .Returns(Guid.NewGuid());
        var handler = new CreateActivityCommandHandler(
            this.activityRepository.Object,
            this.waypointRepository.Object,
            this.currentUserService.Object,
            this.unitOfWork.Object);
        var command = request.Adapt<CreateActivityCommand>();
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.NotEqual(Guid.Empty, result.Value);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenCurrentUserServiceReturnNull()
    {
        //Arrange
        this.currentUserService.Setup(cus => cus.GetCurrentUserId())
            .Returns(Guid.Empty);
        var handler = new CreateActivityCommandHandler(
            this.activityRepository.Object,
            this.waypointRepository.Object,
            this.currentUserService.Object,
            this.unitOfWork.Object);
        var command = new CreateActivityCommand("title", "description", null, 10m, new TimeSpan(10),
            null, null, VisibilityLevelType.All, DateTime.Now,Guid.NewGuid(), 
            new List<string>() { "img1.png" },TestsContants.UserUserId);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.UnauthorizedAccess(nameof(CreateActivityCommand)), result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenCommandIsNotValid()
    {
        //Arrange
        this.currentUserService.Setup(cus => cus.GetCurrentUserId())
            .Returns(Guid.NewGuid());
        var handler = new CreateActivityCommandHandler(
            this.activityRepository.Object,
            this.waypointRepository.Object,
            this.currentUserService.Object,
            this.unitOfWork.Object);
        var command = new CreateActivityCommand("", "description", null, 10m, new TimeSpan(10),
            null, null, VisibilityLevelType.All, DateTime.Now,Guid.NewGuid(), 
            new List<string>() { "img1.png" },TestsContants.UserUserId);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.Activity.TitleIsNullOrEmpty, result.Error);
    }
}