// ------------------------------------------------------------------------------------------------
//  <copyright file="ЦреатеЦомментЦомманд.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Comments.Commands.CreateComment;

using Common.Constants;
using FluentValidation;

public class CreateCommentCommandValidator
    : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        this.RuleFor(c => c.Content)
            .NotEmpty()
            .MinimumLength(GlobalConstants.Comment.ContentMinLength)
            .MaximumLength(GlobalConstants.Comment.ContentMaxLength);
    }
}