// ------------------------------------------------------------------------------------------------
//  <copyright file="ChangePasswordCommand.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Commands.ChangePassword;

using Abstractions.Messaging;

public sealed record ChangePasswordCommand(    
    string UserName,   
    string OldPassword, 
    string NewPassword, 
    string ConfirmNewPassword) : ICommand<bool>;