// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateBikeCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.Bikes.Commands;

using Application.Entities.Bikes.Commands.CreateBike;
using Application.Interfaces;
using Domain.Errors;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Moq;
using Persistence;

public class CreateBikeCommandHandlerTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<IBikeRepository> bikeRepository;
    private readonly Mock<ICurrentUserService> currentUserService;
    private readonly Mock<IUnitOfWork> unitOfWork;

    public CreateBikeCommandHandlerTests()
    {
        this._context = ApplicationDbContextFactory.Create();
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
        Func<CreateBikeCommandHandler> act = () => new CreateBikeCommandHandler(null!,
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
        Func<CreateBikeCommandHandler> act = () => new CreateBikeCommandHandler(
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
        Func<CreateBikeCommandHandler> act = () => new CreateBikeCommandHandler(
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
        this.currentUserService.Setup(cpr => cpr.GetCurrentUserId())
            .Returns(Guid.NewGuid());
        var command = new CreateBikeCommand("Name", Guid.NewGuid(), 10.00m, "Brand",
            "Model", "Notes");
        var handler = new CreateBikeCommandHandler(
            this.bikeRepository.Object,
            this.currentUserService.Object,
            this.unitOfWork.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        var bike = result.Value; 
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenUserIsInvalid()
    {
        //Arrange
        this.currentUserService.Setup(cpr => cpr.GetCurrentUserId())
            .Returns(Guid.Empty);
        var command = new CreateBikeCommand("Name", Guid.NewGuid(), 10.00m, "Brand",
            "Model", "Notes");
        var handler = new CreateBikeCommandHandler(
            this.bikeRepository.Object,
            this.currentUserService.Object,
            this.unitOfWork.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Contains(DomainErrors.UnauthorizedAccess(
            nameof(CreateBikeCommand)), result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenCommandIsInvalid()
    {
        //Arrange
        this.currentUserService.Setup(cpr => cpr.GetCurrentUserId())
            .Returns(Guid.NewGuid);
        var command = new CreateBikeCommand("", Guid.NewGuid(), 10.00m, "Brand",
            "Model", "Notes");
        var handler = new CreateBikeCommandHandler(
            this.bikeRepository.Object,
            this.currentUserService.Object,
            this.unitOfWork.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Contains(DomainErrors.Bike.NameIsNullOrEmpty, result.Error);
    }
}