// ------------------------------------------------------------------------------------------------
//  <copyright file="GetRoleByIdQuery.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Roles.Queries.GetRoleById;

using Abstractions.Messaging;
using Models;

public sealed record GetRoleByIdQuery(Guid Id) : IQuery<RoleResponse>;