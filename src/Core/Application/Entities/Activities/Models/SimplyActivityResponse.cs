// ------------------------------------------------------------------------------------------------
//  <copyright file="SimplyActivityResponse.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Activities.Models;

public sealed record SimplyActivityResponse(
    Guid Id,
    string Title,
    string Description);