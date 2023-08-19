namespace Application.Entities.Activities.Commands.CreateActivity;

using Abstractions.Messaging;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Domain.Shared;
using Interfaces;

public class CreateActivityCommandHandler
    : ICommandHandler<CreateActivityCommand, Guid>
{
    private readonly IActivityRepository _activitiesRepository;
    private readonly IWaypointRepository _waypointsRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateActivityCommandHandler(
        IActivityRepository activitiesRepository,
        IWaypointRepository waypointsRepository, 
        ICurrentUserService currentUserService, 
        IUnitOfWork unitOfWork)
    {
        this._activitiesRepository =
            activitiesRepository ?? throw new ArgumentNullException(nameof(activitiesRepository));
        this._waypointsRepository = waypointsRepository ?? throw new ArgumentNullException(nameof(waypointsRepository));
        this._currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        this._unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<Guid>> Handle(
        CreateActivityCommand request, 
        CancellationToken cancellationToken)
    {
        Guid userId = this._currentUserService.GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return Result.Failure<Guid>(DomainErrors.UnauthorizedAccess(nameof(CreateActivityCommand)));
        }

        var activityResult = Activity.Create(request.Title, request.Description, request.PrivateNotes,
            request.Distance, request.Duration, request.PositiveElevation, request.NegativeElevation,
            request.VisibilityLevel, request.StartDateTime, request.BikeId, userId);
        if (activityResult.IsFailure)
        {
            return Result.Failure<Guid>(activityResult.Error);
        }

        var activity = activityResult.Value;
        activity.AddImages(request.Pictures);
        this._activitiesRepository.Add(activity);
        await this._unitOfWork.SaveChangesAsync(cancellationToken);
        await this._waypointsRepository.UpdateActivityAsync(request.GpxId, activity.Id, cancellationToken);
        return activity.Id;
    }
}