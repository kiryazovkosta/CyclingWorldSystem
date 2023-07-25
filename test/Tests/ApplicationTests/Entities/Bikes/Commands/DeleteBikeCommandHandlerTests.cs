namespace Tests.ApplicationTests.Entities.Bikes.Commands;

using Application.Entities.Bikes.Commands.DeleteBike;
using Domain.Errors;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Moq;
using Persistence;

public class DeleteBikeCommandHandlerTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<IBikeRepository> bikeRepository;
    private readonly Mock<IUnitOfWork> unitOfWork;

    public DeleteBikeCommandHandlerTests()
    {
        this._context = ApplicationDbContextTestFactory.Create();
        this.bikeRepository = new Mock<IBikeRepository>();
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
        Func<DeleteBikeCommandHandler> act = () => new DeleteBikeCommandHandler(
            null!,
            this.unitOfWork.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'bikeRepository')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenUnitOfWorkIsNull()
    {
        //Arrange & Act
        Func<DeleteBikeCommandHandler> act = () => new DeleteBikeCommandHandler(
            this.bikeRepository.Object,
            null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'unitOfWork')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccessWhenCommandIsValid()
    {
        //Arrange
        this.bikeRepository.Setup(cpr => cpr.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));
        var command = new DeleteBikeCommand(Guid.NewGuid());
        var handler = new DeleteBikeCommandHandler(
            this.bikeRepository.Object,
            this.unitOfWork.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenBikeDoesNotExistsIsInvalid()
    {
        //Arrange
        var deletedId = Guid.NewGuid();
        this.bikeRepository.Setup(cpr => cpr.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(false));;
        var command = new DeleteBikeCommand(deletedId);
        var handler = new DeleteBikeCommandHandler(
            this.bikeRepository.Object,
            this.unitOfWork.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.Bike.BikeDoesNotExists(deletedId), result.Error);
    }
}