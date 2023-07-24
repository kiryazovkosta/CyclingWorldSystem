// ------------------------------------------------------------------------------------------------
//  <copyright file="UpdateRoleCommand.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Roles.Commands.UpdateRole;

using Abstractions.Messaging;

public sealed record UpdateRoleCommand(Guid Id, string Name) : ICommand;