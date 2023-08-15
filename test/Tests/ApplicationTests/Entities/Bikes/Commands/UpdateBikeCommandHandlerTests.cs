// ------------------------------------------------------------------------------------------------
//  <copyright file="UpdateBikeCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.Bikes.Commands;

using Application.Entities.Bikes.Commands.UpdateBike;
using Application.Entities.Bikes.Models;
using Application.Interfaces;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Mapster;
using Moq;
using Persistence;

public class UpdateBikeCommandHandlerTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<IBikeRepository> bikeRepository;
    private readonly Mock<ICurrentUserService> currentUserService;
    private readonly Mock<IUnitOfWork> unitOfWork;


    public UpdateBikeCommandHandlerTests()
    {
        this._context = ApplicationDbContextTestFactory.Create();
        this.bikeRepository = new Mock<IBikeRepository>();
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
        Func<UpdateBikeCommandHandler> act = () => new UpdateBikeCommandHandler(
            null!,
            this.currentUserService.Object,
            this.unitOfWork.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'bikeRepository')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenCurrentUserServiceIsNull()
    {
        //Arrange & Act
        Func<UpdateBikeCommandHandler> act = () => new UpdateBikeCommandHandler(
            this.bikeRepository.Object,
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
        Func<UpdateBikeCommandHandler> act = () => new UpdateBikeCommandHandler(
            this.bikeRepository.Object,
            this.currentUserService.Object,
            null!);
    
        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'unitOfWork')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccessWhenCommandIsValid()
    {
        //Arrange
        var fakeBike = this._context.Set<Bike>().FirstOrDefault();
        this.bikeRepository.Setup(br => br.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(fakeBike));
        this.currentUserService.Setup(cpr => cpr.GetCurrentUserId())
            .Returns(fakeBike!.UserId);
        var request = new UpdateBikeRequest(fakeBike.Id, "New name", fakeBike.BikeTypeId, 8.00m, "New brand",
            "New model", "New notes");
        var command = request.Adapt<UpdateBikeCommand>();
        var handler = new UpdateBikeCommandHandler(
            this.bikeRepository.Object,
            this.currentUserService.Object,
            this.unitOfWork.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailedWhenBikeDoesNotExistsWhenCommandIsValid()
    {
        //Arrange
        var updateBikeId = Guid.NewGuid();
        var fakeBike = this._context.Set<Bike>().FirstOrDefault(b => b.Id == Guid.Empty);
        this.bikeRepository.Setup(br => br.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(fakeBike));
        var command = new UpdateBikeCommand(updateBikeId, "New name", Guid.NewGuid(), 8.00m, "New brand",
            "New model", "New notes");
        var handler = new UpdateBikeCommandHandler(
            this.bikeRepository.Object,
            this.currentUserService.Object,
            this.unitOfWork.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Bike.BikeDoesNotExists(command.Id), result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenUserDoesNotExists()
    {
        //Arrange
        var fakeBike = this._context.Set<Bike>().FirstOrDefault();
        this.bikeRepository.Setup(br => br.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(fakeBike));
        this.currentUserService.Setup(cpr => cpr.GetCurrentUserId())
            .Returns(Guid.Empty);
        var command = new UpdateBikeCommand(Guid.NewGuid(), "New name", Guid.NewGuid(), 8.00m, "New brand",
            "New model", "New notes");
        var handler = new UpdateBikeCommandHandler(
            this.bikeRepository.Object,
            this.currentUserService.Object,
            this.unitOfWork.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.UnauthorizedAccess(nameof(UpdateBikeCommand)), result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenUserIsNotOwnerOfBike()
    {
        //Arrange
        var fakeBike = this._context.Set<Bike>().FirstOrDefault();
        this.bikeRepository.Setup(br => br.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(fakeBike));
        this.currentUserService.Setup(cpr => cpr.GetCurrentUserId())
            .Returns(Guid.NewGuid);
        var command = new UpdateBikeCommand(Guid.NewGuid(), "New name", Guid.NewGuid(), 8.00m, "New brand",
            "New model", "New notes");
        var handler = new UpdateBikeCommandHandler(
            this.bikeRepository.Object,
            this.currentUserService.Object,
            this.unitOfWork.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.UnauthorizedAccess(nameof(UpdateBikeCommand)), result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenCommandIsInvalid()
    {
        //Arrange
        var fakeBike = this._context.Set<Bike>().FirstOrDefault();
        this.bikeRepository.Setup(br => br.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(fakeBike));
        this.currentUserService.Setup(cpr => cpr.GetCurrentUserId())
            .Returns(fakeBike!.UserId);
        var command = new UpdateBikeCommand(fakeBike.Id, "", fakeBike.BikeTypeId, 8.00m, "New brand",
            "New model", "New notes");
        var handler = new UpdateBikeCommandHandler(
            this.bikeRepository.Object,
            this.currentUserService.Object,
            this.unitOfWork.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Bike.NameIsNullOrEmpty, result.Error);
    }
    
    
}