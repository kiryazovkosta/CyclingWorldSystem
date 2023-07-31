// ------------------------------------------------------------------------------------------------
//  <copyright file="DeleteCommentCommandHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Comments.Commands.DeleteComment;

using Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Domain.Shared;

public class DeleteCommentCommandHandler
    : ICommandHandler<DeleteCommentCommand>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCommentCommandHandler(
        ICommentRepository commentRepository, 
        IUnitOfWork unitOfWork)
    {
        this._commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        this._unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var deleteResult = await this._commentRepository.DeleteAsync(request.Id, cancellationToken);
        if (!deleteResult)
        {
            return Result.Failure(DomainErrors.Comments.CommentDoesNotExists(request.Id));
        }
		
        await this._unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}