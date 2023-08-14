// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateBikeTypeCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.BikeTypes.Commands;

using Application.Entities.Bikes.Commands.CreateBike;
using Application.Entities.BikeTypes.Commands.CreateBikeType;
using Application.Interfaces;
using Domain.Errors;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Moq;
using Persistence;

public class CreateBikeTypeCommandHandlerTests: IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<IBikeTypeRepository> bikeTypeRepository;
    private readonly Mock<IUnitOfWork> unitOfWork;

    public CreateBikeTypeCommandHandlerTests()
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
        Func<CreateBikeTypeCommandHandler> act = () => new CreateBikeTypeCommandHandler(
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
        Func<CreateBikeTypeCommandHandler> act = () => new CreateBikeTypeCommandHandler(
            this.bikeTypeRepository.Object,
            null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'unitOfWork')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccessWhenCommandIsValid()
    {
        //Arrange
        var command = new CreateBikeTypeCommand("City bike");
        var handler = new CreateBikeTypeCommandHandler(
            this.bikeTypeRepository.Object,
            this.unitOfWork.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        var bikeTypeId = result.Value; 
        Assert.NotEqual(Guid.Empty, bikeTypeId);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenNameExists()
    {
        //Arrange
        this.bikeTypeRepository.Setup(btr => btr.ExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));
        var name = "Road bike";
        var command = new CreateBikeTypeCommand(name);
        var handler = new CreateBikeTypeCommandHandler(
            this.bikeTypeRepository.Object,
            this.unitOfWork.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.BikeType.BikeTypeNameExists(name), result.Error);
    }
    
    
}