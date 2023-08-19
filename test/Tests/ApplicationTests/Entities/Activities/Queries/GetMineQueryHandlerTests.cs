// ------------------------------------------------------------------------------------------------
//  <copyright file="GetMineQueryHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.Activities.Queries;

using Application.Entities.Activities.Queries.GetAll;
using Application.Entities.Activities.Queries.GetMine;
using Application.Interfaces;
using Domain.Errors;
using Domain.Repositories;
using Moq;
using Persistence.Repositories;

public class GetMineQueryHandlerTests : TestApplicationContext
    {
    private readonly IActivityRepository activityRepository;
    private readonly Mock<ICurrentUserService> currentUserService;

    public GetMineQueryHandlerTests()
    {
        this.activityRepository = new ActivityRepository(this.Context);
        this.currentUserService = new Mock<ICurrentUserService>();
    }

    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenActivityRepositoryIsNull()
    {
        //Arrange & Act
        Func<GetMineQueryHandler> act = () => new GetMineQueryHandler(
            this.activityRepository, null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'currentUserService')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenCurrentUserServiceIsNull()
    {
        //Arrange & Act
        Func<GetMineQueryHandler> act = () => new GetMineQueryHandler(
            null!, this.currentUserService.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'activityRepository')", exception.Message);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureWhenCurrentUserIsNotUserFromCommand()
    {
        //Arrange
        this.currentUserService.Setup(cus => cus.GetCurrentUserId())
            .Returns(Guid.NewGuid);
        var handler = new GetMineQueryHandler(this.activityRepository, this.currentUserService.Object);
        var query = new GetMineQuery(TestsContants.UserUserId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.UnauthorizedAccess(nameof(GetMineQuery)), result.Error);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        //Arrange
        this.currentUserService.Setup(cus => cus.GetCurrentUserId())
            .Returns(TestsContants.UserUserId);
        var handler = new GetMineQueryHandler(this.activityRepository, this.currentUserService.Object);
        var query = new GetMineQuery(TestsContants.UserUserId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        var response = result.Value;
        Assert.Equal(4, response.Count);
        var first = response.First();
        Assert.Equal("Trip 1", first.Title);
        Assert.Equal("Description", first.Description);
        Assert.Equal("Private", first.PrivateNotes);
        Assert.Equal(10.00m, first.Distance);
        Assert.Equal(new TimeSpan(0, 1, 0,0), first.Duration);
        Assert.Equal(100.00m, first.PositiveElevation);
        Assert.Equal(99.00m, first.NegativeElevation);
        Assert.Equal(1, first.LikesCount);
        Assert.Equal(2, first.CommentsCount);

    }
}