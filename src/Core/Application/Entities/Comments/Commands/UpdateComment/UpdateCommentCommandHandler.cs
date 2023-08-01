// ------------------------------------------------------------------------------------------------
//  <copyright file="UpdateCommentCommandHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Comments.Commands.UpdateComment;

using Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Domain.Shared;

public class UpdateCommentCommandHandler
    : ICommandHandler<UpdateCommentCommand>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCommentCommandHandler(ICommentRepository commentRepository, IUnitOfWork unitOfWork)
    {
        this._commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        this._unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await this._commentRepository.GetByIdAsync(request.Id, cancellationToken);
        if (comment is null)
        {
            return Result.Failure(DomainErrors.Comments.CommentDoesNotExists(request.Id));
        }

        comment.Update(request.Content);
        await this._unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}