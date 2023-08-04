// ------------------------------------------------------------------------------------------------
//  <copyright file="ConfirmEmailCommand.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Commands.ConfirmEmail;

using Abstractions.Messaging;

public sealed record ConfirmEmailCommand(string UserId, string Code) : ICommand<bool>;