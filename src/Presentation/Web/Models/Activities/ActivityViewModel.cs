﻿// ------------------------------------------------------------------------------------------------
//  <copyright file="ActivityViewModel.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Models.Activities;

using Common.Enumerations;

public class ActivityViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? PrivateNotes { get; set; }
        
    public decimal Distance { get; set; }
        
    public TimeSpan Duration { get; set; }
        
    public decimal? PositiveElevation { get; set; }
        
    public decimal? NegativeElevation { get; set; }
        
    public VisibilityLevelType VisibilityLevel { get; set; }
        
    public DateTime StartDateTime { get; set; }
        
    public string Bike { get; set; } = null!;
    
    public string UserName { get; set; } = null!;
        
    public string Avatar { get; set; } = null!;
    
    public ICollection<string> Images { get; set; } = new List<string>();
}