// ------------------------------------------------------------------------------------------------
//  <copyright file="UpdateUserRolesCommand.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Commands.UpdateUserRoles;

using Abstractions.Messaging;

public sealed record UpdateUserRolesCommand(Guid UserId, string[]? Roles) : ICommand<Guid>;