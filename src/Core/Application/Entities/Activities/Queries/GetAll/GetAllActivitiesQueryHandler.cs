namespace Application.Entities.Activities.Queries.GetAll;

using Abstractions.Messaging;
using Domain.Repositories;
using Domain.Shared;
using Mapster;
using Models;

public class GetAllActivitiesQueryHandler
    : IQueryHandler<GetAllActivitiesQuery, PagedActivityDataResponse>
{
    private readonly IActivityRepository activityRepository;

    public GetAllActivitiesQueryHandler(IActivityRepository activityRepository)
    {
        this.activityRepository = activityRepository ?? throw new ArgumentNullException(nameof(activityRepository));
    }

    public async Task<Result<PagedActivityDataResponse>> Handle(
        GetAllActivitiesQuery request, 
        CancellationToken cancellationToken)
    {
        var activities = 
            await this.activityRepository.GetAllAsync(request.Parameters, cancellationToken);
        var response = activities.Adapt<PagedActivityDataResponse>();
        return Result.Success(response);
    }
}