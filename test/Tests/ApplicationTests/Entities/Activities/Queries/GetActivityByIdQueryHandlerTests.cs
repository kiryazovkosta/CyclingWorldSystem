// ------------------------------------------------------------------------------------------------
//  <copyright file="GetActivityByIdQueryHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.Activities.Queries;

using Application.Entities.Activities.Queries.GetActivityById;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Moq;
using Persistence.Repositories;

public class GetActivityByIdQueryHandlerTests : TestApplicationContext
{
    private readonly IActivityRepository activityRepository;
    private readonly Mock<ICurrentUserService> currentUserService;

    public GetActivityByIdQueryHandlerTests()
    {
        this.activityRepository = new ActivityRepository(this.Context);
        this.currentUserService = new Mock<ICurrentUserService>();
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenActivityRepositoryIsNull()
    {
        //Arrange & Act
        Func<GetActivityByIdQueryHandler> act = () => new GetActivityByIdQueryHandler(
            null!,
            this.currentUserService.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'activityRepository')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenCurrentUserServiceIsNull()
    {
        //Arrange & Act
        Func<GetActivityByIdQueryHandler> act = () => new GetActivityByIdQueryHandler(
            this.activityRepository,
            null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'currentUserService')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnsSingleRecord()
    {
        //Arrange
        var activity = this.Context.Set<Activity>().First();
        var handler =  new GetActivityByIdQueryHandler(this.activityRepository, this.currentUserService.Object);
        var query = new GetActivityByIdQuery(activity.Id);
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        var activityResponse = result.Value;
        Assert.Equal(activity.Id, activityResponse.Id);
        Assert.Equal(activity.Title, activityResponse.Title);
        Assert.Equal(activity.Description, activityResponse.Description);
        Assert.Equal(activity.PrivateNotes, activityResponse.PrivateNotes);
        Assert.Equal(activity.Distance, activityResponse.Distance);
        Assert.Equal(activity.Duration, activityResponse.Duration);
        Assert.Equal(activity.PositiveElevation, activityResponse.PositiveElevation);
        Assert.Equal(activity.NegativeElevation, activityResponse.NegativeElevation);
        Assert.Equal(activity.VisibilityLevel, activityResponse.VisibilityLevel);
        Assert.Equal(activity.StartDateTime, activityResponse.StartDateTime);
        Assert.Equal(activity.Title, activityResponse.Title);
        Assert.Equal(2, activityResponse.Comments.Count());
        var firstComment = activityResponse.Comments.First();
        Assert.Equal("Comment content", firstComment.Content);
    }
}