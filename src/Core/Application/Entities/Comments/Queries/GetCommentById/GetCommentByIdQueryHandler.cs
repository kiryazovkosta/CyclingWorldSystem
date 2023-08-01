// ------------------------------------------------------------------------------------------------
//  <copyright file="GetCommentByIdQueryHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Comments.Queries.GetCommentById;

using Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Mapster;
using Models;

public class GetCommentByIdQueryHandler
    : IQueryHandler<GetCommentByIdQuery, CommentResponse>
{
    private readonly ICommentRepository _commentRepository;

    public GetCommentByIdQueryHandler(ICommentRepository commentRepository)
    {
        this._commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
    }

    public async Task<Result<CommentResponse>> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
    {
        var comment = await this._commentRepository.GetByIdAsync(request.Id, cancellationToken);
        if (comment is null)
        {
            return Result.Failure<CommentResponse>(DomainErrors.Comments.CommentDoesNotExists(request.Id));
        }
        
        var response = comment.Adapt<CommentResponse>();
        return response; 
    }
}