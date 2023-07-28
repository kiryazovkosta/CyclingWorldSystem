// ------------------------------------------------------------------------------------------------
//  <copyright file="IActivityLikeRepository.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Domain.Repositories;

using Entities;

public interface IActivityLikeRepository
{
    void Add(ActivityLike activityLike);
}