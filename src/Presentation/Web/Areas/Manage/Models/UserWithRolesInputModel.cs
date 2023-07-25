// ------------------------------------------------------------------------------------------------
//  <copyright file="UserWithRolesInputModel.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Areas.Manage.Models;

public class UserWithRolesInputModel
{
    public Guid UserId { get; set; }

    public string[]? Roles { get; set; }
}