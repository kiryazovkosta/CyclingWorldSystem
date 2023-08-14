// ------------------------------------------------------------------------------------------------
//  <copyright file="DeleteBikeTypeCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.BikeTypes.Commands;

using Application.Entities.BikeTypes.Commands.DeleteBikeType;
using Domain.Errors;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Moq;
using Persistence;

public class DeleteBikeTypeCommandHandlerTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<IBikeTypeRepository> bikeTypeRepository;
    private readonly Mock<IUnitOfWork> unitOfWork;

    public DeleteBikeTypeCommandHandlerTests()
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
        Func<DeleteBikeTypeCommandHandler> act = () => new DeleteBikeTypeCommandHandler(
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
        Func<DeleteBikeTypeCommandHandler> act = () => new DeleteBikeTypeCommandHandler(
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
        this.bikeTypeRepository.Setup(cpr => cpr.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));
        var command = new DeleteBikeTypeCommand(Guid.NewGuid());
        var handler = new DeleteBikeTypeCommandHandler(
            this.bikeTypeRepository.Object,
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
        this.bikeTypeRepository.Setup(cpr => cpr.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(false));;
        var command = new DeleteBikeTypeCommand(deletedId);
        var handler = new DeleteBikeTypeCommandHandler(
            this.bikeTypeRepository.Object,
            this.unitOfWork.Object);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.DeleteOperationFailed(command.Id, "DeleteBikeCommandHandler"), result.Error);
    }
    
}