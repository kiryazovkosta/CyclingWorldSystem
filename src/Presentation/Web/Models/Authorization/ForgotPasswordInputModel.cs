// ------------------------------------------------------------------------------------------------
//  <copyright file="ForgotPasswordInputModel.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Models.Authorization;

public class ForgotPasswordInputModel
{
    public string UserName { get; set; } = null!;
}