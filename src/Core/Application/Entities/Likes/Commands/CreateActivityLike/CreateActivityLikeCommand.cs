// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateActivityLikeCommand.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Likes.Commands.CreateActivityLike;

using Abstractions.Messaging;

public sealed record CreateActivityLikeCommand(Guid ActivityId, Guid UserId) : ICommand<bool>;