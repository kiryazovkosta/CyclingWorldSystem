// ------------------------------------------------------------------------------------------------
//  <copyright file="ResetPasswordInputModel.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Models.Authorization;

public class ResetPasswordInputModel
{
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string ConfirmPassword { get; set; } = null!;

    public string Code { get; set; } = null!; 
}