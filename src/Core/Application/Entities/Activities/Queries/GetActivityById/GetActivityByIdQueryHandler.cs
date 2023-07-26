namespace Application.Entities.Activities.Queries.GetActivityById;

using Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Mapster;
using Models;

public class GetActivityByIdQueryHandler
    : IQueryHandler<GetActivityByIdQuery, ActivityResponse>
{
    private readonly IActivityRepository activityRepository;

    public GetActivityByIdQueryHandler(IActivityRepository activityRepository)
    {
        this.activityRepository = activityRepository ?? throw new ArgumentNullException(nameof(activityRepository));
    }

    public async Task<Result<ActivityResponse>> Handle(
        GetActivityByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var activity = await this.activityRepository.GetByIdAsync(request.Id, cancellationToken);
        if (activity is null)
        {
            return Result.Failure<ActivityResponse>(
                DomainErrors.Activity.ActivityDoesNotExists(request.Id));
        }

        var response = activity.Adapt<ActivityResponse>();
        return response;
    }
}