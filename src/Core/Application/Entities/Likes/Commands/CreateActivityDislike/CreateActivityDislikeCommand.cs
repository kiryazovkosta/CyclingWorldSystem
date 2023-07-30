// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateActivityDislikeCommand.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Likes.Commands.CreateActivityDislike;

using Abstractions.Messaging;

public sealed record CreateActivityDislikeCommand(Guid ActivityId, Guid UserId) : ICommand<bool>;