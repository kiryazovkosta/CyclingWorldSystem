// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateCommentCommand.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Comments.Commands.CreateComment;

using Abstractions.Messaging;

public sealed record CreateCommentCommand(Guid ActivityId, string Content) : ICommand<Guid>;