// ------------------------------------------------------------------------------------------------
//  <copyright file="ResetPasswordCommand.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Commands.ResetPassword;

using Abstractions.Messaging;

public sealed record ResetPasswordCommand(string UserId,string Code,string Password, string ConfirmPassword) : ICommand<bool>;