// ------------------------------------------------------------------------------------------------
//  <copyright file="GetCommentByIdQuery.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Comments.Queries.GetCommentById;

using Abstractions.Messaging;
using Models;

public sealed record GetCommentByIdQuery(Guid Id) : IQuery<CommentResponse>;