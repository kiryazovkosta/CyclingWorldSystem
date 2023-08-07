// ------------------------------------------------------------------------------------------------
//  <copyright file="UserViewModel.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Models.Users;

public sealed record UserProfileViewModel(
    string Id,
    string UserName, 
    string Email, 
    string FirstName, 
    string LastName, 
    string OldPassword, 
    string NewPassword, 
    string ConfirmNewPassword);