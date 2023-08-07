// ------------------------------------------------------------------------------------------------
//  <copyright file="ForgotPasswordCommand.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Commands.ForgotPassword;

using Abstractions.Messaging;

public sealed record ForgotPasswordCommand(string UserName) : ICommand<bool>;