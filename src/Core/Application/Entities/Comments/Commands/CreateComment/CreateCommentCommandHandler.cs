// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateCommentCommandHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Comments.Commands.CreateComment;

using Abstractions.Messaging;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Domain.Shared;
using Interfaces;

public class CreateCommentCommandHandler
    : ICommandHandler<CreateCommentCommand, Guid>
{
    private readonly IActivityRepository _activityRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCommentCommandHandler(
        IActivityRepository activityRepository, 
        ICommentRepository commentRepository,
        ICurrentUserService currentUserService, 
        IUnitOfWork unitOfWork)
    {
        this._activityRepository = activityRepository ?? throw new ArgumentNullException(nameof(activityRepository));
        this._commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        this._currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        this._unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<Guid>> Handle(
        CreateCommentCommand request, 
        CancellationToken cancellationToken)
    {
        var activityExists = await this._activityRepository.ExistsAsync(request.ActivityId, cancellationToken);
        if (!activityExists)
        {
            return Result.Failure<Guid>(DomainErrors.Activity.ActivityDoesNotExists(request.ActivityId));
        }

        var userId =this._currentUserService.GetCurrentUserId();

        var commentResult = Comment.Create(request.ActivityId, userId, request.Content);
        if (commentResult.IsFailure)
        {
            return Result.Failure<Guid>(commentResult.Error);
        }

        var comment = commentResult.Value;
        this._commentRepository.Add(comment);
        await this._unitOfWork.SaveChangesAsync(cancellationToken);
        return comment.Id;
    }
}