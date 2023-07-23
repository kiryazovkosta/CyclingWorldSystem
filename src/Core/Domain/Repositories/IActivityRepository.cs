// ------------------------------------------------------------------------------------------------
//  <copyright file="IActivityRepository.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Domain.Repositories;

using Entities;

public interface IActivityRepository
{
    void Add(Activity activity);
}