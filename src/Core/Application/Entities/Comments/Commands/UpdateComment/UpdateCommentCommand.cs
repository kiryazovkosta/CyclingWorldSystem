﻿// ------------------------------------------------------------------------------------------------
//  <copyright file="UpdateCommentCommand.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Comments.Commands.UpdateComment;

using Abstractions.Messaging;

public sealed record UpdateCommentCommand(Guid Id, string Content) : ICommand;