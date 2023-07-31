// ------------------------------------------------------------------------------------------------
//  <copyright file="DeleteCommentCommandValidator.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Comments.Commands.DeleteComment;

using FluentValidation;

public class DeleteCommentCommandValidator
    : AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentCommandValidator()
    {
        
    }
}