// ------------------------------------------------------------------------------------------------
//  <copyright file="GetRolesByUserIdCommand.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Queries.GetUserRoles;

using Application.Abstractions.Messaging;

public sealed record GetUserRolesCommand(Guid UserId) : ICommand<IEnumerable<string>>;