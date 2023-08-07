// ------------------------------------------------------------------------------------------------
//  <copyright file="ChangePasswordRequest.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Models;

public sealed record ChangePasswordRequest(
    string UserName,   
    string OldPassword, 
    string NewPassword, 
    string ConfirmNewPassword);