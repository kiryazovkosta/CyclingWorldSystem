// ------------------------------------------------------------------------------------------------
//  <copyright file="ExistsBikeTypeByIsQueryHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.BikeTypes.Queries;

using System.Linq.Dynamic.Core;
using Application.Entities.Bikes.Queries.GetBikeById;
using Application.Entities.BikeTypes.Queries.ExistsBikeTypeById;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Persistence.Repositories;

public class ExistsBikeTypeByIsQueryHandlerTests : TestApplicationContext
{
    private readonly IBikeTypeRepository bikeTypeRepository;

    public ExistsBikeTypeByIsQueryHandlerTests()
    {
        this.bikeTypeRepository = new BikeTypeRepository(this.Context);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<ExistsBikeTypeByIsQueryHandler> act = () => new ExistsBikeTypeByIsQueryHandler(null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'bikeTypeRepository')", exception.Message);
    } 
    
    [Fact]
    public async Task Handle_Should_ReturnTrueWhenExists()
    {
        //Arrange
        var bikeType = this.Context.Set<BikeType>().First();
        var handler =  new ExistsBikeTypeByIsQueryHandler(this.bikeTypeRepository);
        var query = new ExistsBikeTypeByIsQuery(bikeType.Id);
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.True(result.Value);
    } 
    
    [Fact]
    public async Task Handle_Should_ReturnFalseWhenDoesNotExists()
    {
        //Arrange
        var handler =  new ExistsBikeTypeByIsQueryHandler(this.bikeTypeRepository);
        var query = new ExistsBikeTypeByIsQuery(Guid.NewGuid());
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.False(result.Value);
    } 
}