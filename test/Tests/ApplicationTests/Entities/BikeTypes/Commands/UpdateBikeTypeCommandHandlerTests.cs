// ------------------------------------------------------------------------------------------------
//  <copyright file="UpdateBikeTypeCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.BikeTypes.Commands;

using Application.Entities.BikeTypes.Commands.UpdateBikeType;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Moq;
using Persistence;

public class UpdateBikeTypeCommandHandlerTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<IBikeTypeRepository> bikeTypeRepository;
    private readonly Mock<IUnitOfWork> unitOfWork;

    public UpdateBikeTypeCommandHandlerTests()
    {
        this._context = ApplicationDbContextTestFactory.Create();
        this.bikeTypeRepository = new Mock<IBikeTypeRepository>();
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
        Func<UpdateBikeTypeCommandHandler> act = () => new UpdateBikeTypeCommandHandler(
            null!,
            this.unitOfWork.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'bikeTypeRepository')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenUnitOfWorkIsNull()
    {
        //Arrange & Act
        Func<UpdateBikeTypeCommandHandler> act = () => new UpdateBikeTypeCommandHandler(
            this.bikeTypeRepository.Object,
            null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'unitOfWork')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenBikeTypeDoesNotExists()
    {
        //Arrange
        this.bikeTypeRepository.Setup(btr => btr.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<BikeType?>(null));
        var command = new UpdateBikeTypeCommand(Guid.NewGuid(), "City bike");
        var handler = new UpdateBikeTypeCommandHandler(
            this.bikeTypeRepository.Object,
            this.unitOfWork.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.BikeType.BikeTypeDoesNotExists, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenNameAlreadyExists()
    {
        //Arrange
        var bikeType = this._context.Set<BikeType>().First();
        this.bikeTypeRepository.Setup(btr => btr.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<BikeType?>(bikeType));
        this.bikeTypeRepository.Setup(btr => btr.ExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));
        var command = new UpdateBikeTypeCommand(Guid.NewGuid(), "City bike");
        var handler = new UpdateBikeTypeCommandHandler(
            this.bikeTypeRepository.Object,
            this.unitOfWork.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.BikeType.BikeTypeNameExists("City bike"), result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenCommandIsInvalid()
    {
        //Arrange
        var bikeType = this._context.Set<BikeType>().First();
        this.bikeTypeRepository.Setup(btr => btr.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<BikeType?>(bikeType));
        this.bikeTypeRepository.Setup(btr => btr.ExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(false));
        var command = new UpdateBikeTypeCommand(Guid.NewGuid(), "");
        var handler = new UpdateBikeTypeCommandHandler(
            this.bikeTypeRepository.Object,
            this.unitOfWork.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.BikeType.BikeTypeNameIsNull, result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccessWhenInputIsValid()
    {
        //Arrange
        var bikeType = this._context.Set<BikeType>().First();
        this.bikeTypeRepository.Setup(btr => btr.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<BikeType?>(bikeType));
        this.bikeTypeRepository.Setup(btr => btr.ExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(false));
        var command = new UpdateBikeTypeCommand(Guid.NewGuid(), "New name");
        var handler = new UpdateBikeTypeCommandHandler(
            this.bikeTypeRepository.Object,
            this.unitOfWork.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }
}