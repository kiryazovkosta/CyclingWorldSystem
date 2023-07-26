// ------------------------------------------------------------------------------------------------
//  <copyright file="SimpleActivityViewModel.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Models.Activities;

public sealed record SimpleActivityViewModel(
    Guid Id,
    string UserName,
    string Title,
    string Description,
    decimal Distance,
    decimal PositiveElevation,
    TimeSpan Duration,
    string Avatar,
    string FirstPicture);