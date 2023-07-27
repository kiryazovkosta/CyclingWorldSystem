namespace Application.Entities.Activities.Queries.GetActivityById;

using Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Interfaces;
using Mapster;
using Models;

public class GetActivityByIdQueryHandler
    : IQueryHandler<GetActivityByIdQuery, ActivityResponse>
{
    private readonly IActivityRepository _activityRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetActivityByIdQueryHandler(IActivityRepository activityRepository, ICurrentUserService currentUserService)
    {
        this._activityRepository = activityRepository ?? throw new ArgumentNullException(nameof(activityRepository));
        this._currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
    }

    public async Task<Result<ActivityResponse>> Handle(
        GetActivityByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var activity = await this._activityRepository.GetByIdAsync(request.Id, cancellationToken);
        if (activity is null)
        {
            return Result.Failure<ActivityResponse>(
                DomainErrors.Activity.ActivityDoesNotExists(request.Id));
        }

        var response = activity.Adapt<ActivityResponse>();
        response.IsLikedByMe = activity.Likes.Any(l => l.UserId == this._currentUserService.GetCurrentUserId());
        return response;
    }
}