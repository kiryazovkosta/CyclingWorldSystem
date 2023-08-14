using Moq;
using Persistence.Repositories.Abstractions;
using Persistence;

namespace Tests.InfrastructureTests.Persistance.Repositories;

public class UnitOfWorkTests
{
    [Fact]
    public async Task UnitOfWork_SaveChangesAsync_CallsDbContextSaveChangesAsync()
    {
        // Arrange
        var dbContextMock = new Mock<ApplicationDbContext>();
        var unitOfWork = new UnitOfWork(dbContextMock.Object);

        // Act
        await unitOfWork.SaveChangesAsync();

        // Assert
        dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UnitOfWork_SaveChangesAsync_CallsDbContextSaveChangesAsyncWithProvidedCancellationToken()
    {
        // Arrange
        var dbContextMock = new Mock<ApplicationDbContext>();
        var unitOfWork = new UnitOfWork(dbContextMock.Object);
        var cancellationToken = new CancellationTokenSource().Token;

        // Act
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Assert
        dbContextMock.Verify(db => db.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public void UnitOfWork_Constructor_NullDbContext_ThrowsArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new UnitOfWork(null));
    }
}
