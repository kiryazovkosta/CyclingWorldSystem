// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateActivityDislikeCommandHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Likes.Commands.CreateActivityDislike;

using Abstractions.Messaging;
using Domain.Entities;
using Domain.Errors;
using Domain.Identity;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Domain.Shared;
using Microsoft.AspNetCore.Identity;

public class CreateActivityDislikeCommandHandler
    : ICommandHandler<CreateActivityDislikeCommand, bool>
{
    private readonly IActivityRepository _activityRepository;
    private readonly IActivityLikeRepository _likeRepository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public CreateActivityDislikeCommandHandler(IActivityRepository activityRepository,
        IActivityLikeRepository likeRepository, UserManager<User> userManager, IUnitOfWork unitOfWork)
    {
        this._activityRepository = activityRepository ?? throw new ArgumentNullException(nameof(activityRepository));
        this._likeRepository = likeRepository ?? throw new ArgumentNullException(nameof(likeRepository));
        this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this._unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<bool>> Handle(
        CreateActivityDislikeCommand request, 
        CancellationToken cancellationToken)
    {
        var activityExists = await this._activityRepository.ExistsAsync(request.ActivityId, cancellationToken);
        if (!activityExists)
        {
            return Result.Failure<bool>(DomainErrors.Activity.ActivityDoesNotExists(request.ActivityId));
        } 
        
        var user = await this._userManager.FindByIdAsync(request.UserId.ToString());
        if(user is null) 
        {
            return Result.Failure<bool>(DomainErrors.User.NonExistsUser);
        }

        var like = await this._likeRepository.GetByIdAsync(request.ActivityId, request.UserId, cancellationToken);
        if (like is null)
        {
            return Result.Failure<bool>(DomainErrors.ActivityLike.LikeDoesNotExists(request.ActivityId));
        }

        this._likeRepository.Remove(like);
        await this._unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(true);
    }
}