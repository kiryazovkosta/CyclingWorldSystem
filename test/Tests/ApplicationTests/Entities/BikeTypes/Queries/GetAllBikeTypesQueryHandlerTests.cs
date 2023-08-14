// ------------------------------------------------------------------------------------------------
//  <copyright file="GetAllBikeTypesQueryHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.BikeTypes.Queries;

using Application.Entities.BikeTypes.Queries.GetAllBikeTypes;
using Domain.Entities;
using Domain.Repositories;
using Persistence.Repositories;

public class GetAllBikeTypesQueryHandlerTests : TestApplicationContext
{
    private readonly IBikeTypeRepository bikeTypeRepository;

    public GetAllBikeTypesQueryHandlerTests()
    {
        this.bikeTypeRepository = new BikeTypeRepository(this.Context);
    }

    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<GetAllBikeTypesQueryHandler> act = () => new GetAllBikeTypesQueryHandler(null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'bikeTypeRepository')", exception.Message);
    }

    [Fact]
    public async Task Handle_Should_ReturnTrueWhenExists()
    {
        //Arrange
        var bikeType = this.Context.Set<BikeType>().First();
        var handler = new GetAllBikeTypesQueryHandler(this.bikeTypeRepository);
        var query = new GetAllBikeTypesQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        var bikeTypeResult = result.Value;
        Assert.Equal(2, result.Value.Count());
    }
}