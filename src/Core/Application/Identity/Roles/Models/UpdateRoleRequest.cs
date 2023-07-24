// ------------------------------------------------------------------------------------------------
//  <copyright file="UpdateRoleRequest.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Roles.Models;

public sealed record UpdateRoleRequest(Guid Id, string Name);