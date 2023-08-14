// ------------------------------------------------------------------------------------------------
//  <copyright file="GetBikeByIdQueryHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.Bikes.Queries;

using Application.Entities.Bikes.Queries.GetBikeById;
using Domain.Entities;
using Domain.Repositories;
using Moq;
using Persistence.Repositories;

public class GetBikeByIdQueryHandlerTests : TestApplicationContext
{
    private readonly IBikeRepository bikeRepository;

    public GetBikeByIdQueryHandlerTests()
    {
        this.bikeRepository = new BikeRepository(this.Context);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<GetBikeByIdQueryHandler> act = () => new GetBikeByIdQueryHandler(null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'bikeRepository')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnsRecord()
    {
        //Arrange
        var bike = this.Context.Set<Bike>().First();
        var handler =  new GetBikeByIdQueryHandler(this.bikeRepository);
        var query = new GetBikeByIdQuery(bike.Id);
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        var bikeResponse = result.Value;
        Assert.Equal(bike.Name, bikeResponse.Name);
        Assert.Equal(bike.Brand, bikeResponse.Brand);
        Assert.Equal(bike.Model, bikeResponse.Model);
        Assert.Equal(bike.Weight, bikeResponse.Weight);
        Assert.Equal(bike.Notes, bikeResponse.Notes);
        Assert.Equal(bike.UserId, bikeResponse.UserId);
        Assert.Equal(bike.BikeTypeId, bikeResponse.BikeTypeId);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnsFailure()
    {
        //Arrange
        var handler =  new GetBikeByIdQueryHandler(this.bikeRepository);
        var query = new GetBikeByIdQuery(Guid.NewGuid());
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        //Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
    }
}