// ------------------------------------------------------------------------------------------------
//  <copyright file="GetAllRolesQuery.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Roles.Queries.GetAllRoles;

using Abstractions.Messaging;
using Models;

public sealed record GetAllRolesQuery() : IQuery<List<RoleResponse>>;