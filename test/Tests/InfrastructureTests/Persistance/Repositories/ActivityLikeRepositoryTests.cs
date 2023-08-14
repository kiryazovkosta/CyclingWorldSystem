using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;
using Persistence.Repositories;

namespace Tests.InfrastructureTests.Persistance.Repositories;

public class ActivityLikeRepositoryTests
{
    private readonly ApplicationDbContext _context;

    public ActivityLikeRepositoryTests()
    {
        this._context = ApplicationDbContextTestFactory.Create();
    }

    [Fact]
    public async Task ActivityLikeRepository_GetByIdAsync_ReturnsExpectedResult()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
        var activityLike = new ActivityLike { ActivityId = activityId, UserId = userId };

        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var dbContext = new ApplicationDbContext(dbContextOptions);
        dbContext.Add(activityLike);
        dbContext.SaveChanges();

        var repository = new ActivityLikeRepository(dbContext);

        // Act
        var result = await repository.GetByIdAsync(activityId, userId, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(activityId, result.ActivityId);
        Assert.Equal(userId, result.UserId);
    }

    [Fact]
    public void ActivityLikeRepository_Add_CallsDbContextSetAdd()
    {
        // Arrange
        var repository = new ActivityLikeRepository(this._context);
        var activityLike = new ActivityLike();

        // Act
        repository.Add(activityLike);
    }

    //[Fact]
    //public void ActivityLikeRepository_Remove_CallsDbContextSetRemove()
    //{
    //    // Arrange
    //    var dbContextMock = new Mock<ApplicationDbContext>();
    //    var repository = new ActivityLikeRepository(this._context);
    //    var activityLike = new ActivityLike();

    //    // Act
    //    repository.Remove(activityLike);
    //}

    [Fact]
    public void ActivityLikeRepository_Constructor_NullDbContext_ThrowsArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ActivityLikeRepository(null!));
    }
}
