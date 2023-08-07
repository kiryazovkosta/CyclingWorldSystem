// ------------------------------------------------------------------------------------------------
//  <copyright file="ResendConfirmEmailCommand.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Commands.ResendConfirmEmail;

using Abstractions.Messaging;

public sealed record ResendConfirmEmailCommand(string Email) : ICommand<bool>;