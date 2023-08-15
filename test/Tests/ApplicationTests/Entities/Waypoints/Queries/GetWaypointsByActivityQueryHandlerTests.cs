// ------------------------------------------------------------------------------------------------
//  <copyright file="GetWaypointsByActivityQueryHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.Waypoints.Queries;

using System.Linq.Dynamic.Core;
using Application.Entities.Waypoints.Queries.GetWaypointsByActivity;
using Domain.Entities;
using Domain.Repositories;
using Persistence.Repositories;

public class GetWaypointsByActivityQueryHandlerTests: TestApplicationContext
{
    private readonly IWaypointRepository waypointRepository;

    public GetWaypointsByActivityQueryHandlerTests()
    {
        this.waypointRepository = new WaypointRepository(this.Context);
    }
    
    
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<GetWaypointsByActivityQueryHandler> act = () => new GetWaypointsByActivityQueryHandler(null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'waypointRepository')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnCollectionOfWaypoints()
    {
        //Arrange & Act
        Activity activity = this.Context.Set<Activity>().First();
        var handler = new GetWaypointsByActivityQueryHandler(this.waypointRepository);
        var command = new GetWaypointsByActivityQuery(activity.Id);

        //Assert
        var result = await handler.Handle(command, CancellationToken.None);
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(5, result.Value.Count());
        Assert.Equal(10.20m, result.Value.First().Latitude);
        Assert.Equal(30.30m, result.Value.First().Longitude);
    }
}