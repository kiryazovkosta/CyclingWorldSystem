// ------------------------------------------------------------------------------------------------
//  <copyright file="GetAllActivitiesQueryHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.Activities.Queries;

using Application.Entities.Activities.Queries.GetAll;
using Domain;
using Domain.Repositories;
using Persistence.Repositories;

public class GetAllActivitiesQueryHandlerTests: TestApplicationContext
{
    private readonly IActivityRepository activityRepository;

    public GetAllActivitiesQueryHandlerTests()
    {
        this.activityRepository = new ActivityRepository(this.Context);
    }

    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenBikeRepositoryIsNull()
    {
        //Arrange & Act
        Func<GetAllActivitiesQueryHandler> act = () => new GetAllActivitiesQueryHandler(null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'activityRepository')", exception.Message);
    }

    [Fact]
    public async Task Handle_Should_ReturnTrueWhenExists()
    {
        //Arrange
        var parameters = new QueryParameter()
            { PageSize = 10, PageNumber = 1, SearchBy = null, OrderBy = null, FilterBy = null };
        var handler = new GetAllActivitiesQueryHandler(this.activityRepository);
        var query = new GetAllActivitiesQuery(parameters);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        var activitiesResult = result.Value;
        Assert.Equal(4, activitiesResult.Items.Count());
    }
}