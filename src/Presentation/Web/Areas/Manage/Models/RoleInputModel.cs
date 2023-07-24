// ------------------------------------------------------------------------------------------------
//  <copyright file="RoleInputModel.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Areas.Manage.Models;

public class RoleInputModel
{
    public string? Id { get; set; }
    public string Name { get; set; } = null!;
    public string UserId { get; set; } = null!;
}