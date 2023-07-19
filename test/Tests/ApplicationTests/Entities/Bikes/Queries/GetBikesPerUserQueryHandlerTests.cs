// ------------------------------------------------------------------------------------------------
//  <copyright file="GetBikesPerUserQueryHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.Bikes.Queries;

using Application.Entities.Bikes.Queries.GetBikesPerUser;
using Domain.Repositories;
using Persistence.Repositories;

public class GetBikesPerUserQueryHandlerTests : TestApplicationContext
{
    private readonly IBikeRepository bikeRepository;

    public GetBikesPerUserQueryHandlerTests()
    {
        this.bikeRepository = new BikeRepository(this.Context);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<GetBikesPerUserQueryHandler> act = () => new GetBikesPerUserQueryHandler(null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'bikeRepository')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnsCollectionOfRecords()
    {
        //Arrange
        var handler =  new GetBikesPerUserQueryHandler(this.bikeRepository);
        var query = new GetBikesPerUserQuery(TestsContants.UserUserId);
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(3, result.Value.Count());
    }
    
    [Fact]
    public async Task Handle_Should_ReturnsEmptyCollectionOfRecords()
    {
        //Arrange
        var handler =  new GetBikesPerUserQueryHandler(this.bikeRepository);
        var query = new GetBikesPerUserQuery(Guid.NewGuid());
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Empty(result.Value);
    }
    
}