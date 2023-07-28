// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateActivityLikeRequest.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Likes.Models;

public sealed record CreateActivityLikeRequest(Guid ActivityId, Guid UserId);