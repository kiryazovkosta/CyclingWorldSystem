// ------------------------------------------------------------------------------------------------
//  <copyright file="UpdateCommentCommandValidator.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Comments.Commands.UpdateComment;

using Common.Constants;
using FluentValidation;

public class UpdateCommentCommandValidator
    : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentCommandValidator()
    {
        this.RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
        
        this.RuleFor(c => c.Content)
            .NotEmpty()
            .MinimumLength(GlobalConstants.Comment.ContentMinLength)
            .MaximumLength(GlobalConstants.Comment.ContentMaxLength);
    }
}