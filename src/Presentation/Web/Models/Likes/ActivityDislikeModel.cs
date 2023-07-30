// ------------------------------------------------------------------------------------------------
//  <copyright file="ActivityDislikeModel.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Models.Likes;

public sealed record ActivityDislikeModel(Guid ActivityId, Guid UserId);