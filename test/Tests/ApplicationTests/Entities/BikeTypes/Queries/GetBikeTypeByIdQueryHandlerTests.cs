// ------------------------------------------------------------------------------------------------
//  <copyright file="GetBikeTypeByIdQueryHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.BikeTypes.Queries;

using Application.Entities.BikeTypes.Queries.GetBikeTypeById;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Persistence.Repositories;

public class GetBikeTypeByIdQueryHandlerTests : TestApplicationContext
{
    private readonly IBikeTypeRepository bikeTypeRepository;

    public GetBikeTypeByIdQueryHandlerTests()
    {
        this.bikeTypeRepository = new BikeTypeRepository(this.Context);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func< GetBikeTypeByIdQueryHandler> act = () => new  GetBikeTypeByIdQueryHandler(null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'bikeTypeRepository')", exception.Message);
    } 
    
    [Fact]
    public async Task Handle_Should_ReturnTrueWhenExists()
    {
        //Arrange
        var bikeType = this.Context.Set<BikeType>().First();
        var handler =  new  GetBikeTypeByIdQueryHandler(this.bikeTypeRepository);
        var query = new  GetBikeTypeByIdQuery(bikeType.Id);
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        var bikeTypeResult = result.Value;
        Assert.Equal(bikeType.Id, bikeTypeResult.Id);
    } 
    
    [Fact]
    public async Task Handle_Should_ReturnFalseWhenDoesNotExists()
    {
        //Arrange
        var handler =  new  GetBikeTypeByIdQueryHandler(this.bikeTypeRepository);
        var query = new  GetBikeTypeByIdQuery(Guid.NewGuid());
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Contains(DomainErrors.BikeType.BikeTypeDoesNotExists, result.Error);
    } 
}