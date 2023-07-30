// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateCommentRequest.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Comments.Models;

public sealed record CreateCommentRequest(Guid ActivityId, string Content);