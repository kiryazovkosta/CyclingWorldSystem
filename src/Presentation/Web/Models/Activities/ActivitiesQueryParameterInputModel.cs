// ------------------------------------------------------------------------------------------------
//  <copyright file="ActivitiesQueryParameterInputModel.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Models.Activities;

public class ActivitiesQueryParameterInputModel
{
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
    public string? OrderBy { get; init; }
    public string? FilterBy { get; init; }
}