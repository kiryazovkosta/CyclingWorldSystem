// ------------------------------------------------------------------------------------------------
//  <copyright file="ResultMessageModel.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Models;

public class ResultMessageModel
{
    public string? Error { get; set; }
    public string? Success { get; set; }
}