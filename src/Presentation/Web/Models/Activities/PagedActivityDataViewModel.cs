// ------------------------------------------------------------------------------------------------
//  <copyright file="PagedActivityDataViewModel.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Models.Activities;

public class PagedActivityDataViewModel
{
    public int CurrentPage { get; init; }
    public int TotalPages { get; init; }
    public int PageSize { get; init; }
    public int TotalItemsCount { get; init; }
    public bool HasPrevious { get; init; }
    public bool HasNext { get; init; }

    public List<SimpleActivityViewModel> Items { get; init; } = new List<SimpleActivityViewModel>();
}