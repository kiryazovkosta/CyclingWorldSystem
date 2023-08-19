namespace Application.Entities.Activities.Queries.GetMine;

using Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Interfaces;
using Mapster;
using Models;

public class GetMineQueryHandler
    : IQueryHandler<GetMineQuery, List<MyActivityResponse>>
{
    private readonly IActivityRepository _activityRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetMineQueryHandler(IActivityRepository activityRepository, ICurrentUserService currentUserService)
    {
        this._activityRepository = activityRepository ?? throw new ArgumentNullException(nameof(activityRepository));
        this._currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
    }

    public async Task<Result<List<MyActivityResponse>>> Handle(GetMineQuery request, CancellationToken cancellationToken)
    {
        if (this._currentUserService.GetCurrentUserId() != request.UserId)
        {
            return Result.Failure<List<MyActivityResponse>>(DomainErrors.UnauthorizedAccess(nameof(GetMineQuery)));
        }
        
        var activities = await this._activityRepository.GetAllMineAsync(request.UserId, cancellationToken);
        var response = activities.Adapt<List<MyActivityResponse>>();
        return response;
    }
}