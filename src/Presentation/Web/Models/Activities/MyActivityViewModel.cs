// ------------------------------------------------------------------------------------------------
//  <copyright file="MyActivityViewModel.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Models.Activities;

using System.Runtime.InteropServices.JavaScript;

public class MyActivityViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? PrivateNotes { get; set; }
    public decimal Distance { get; set; }
    public TimeSpan Duration { get; set; }
    public decimal? PositiveElevation { get; set; }
    public decimal? NegativeElevation { get; set; }
    public DateTime StartDateTime { get; set; }
    public string Bike { get; set; } = null!;
    public int LikeCount { get; set; }
    public DateTime CreatedOn { get; set; }
}