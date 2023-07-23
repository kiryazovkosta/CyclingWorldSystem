// ------------------------------------------------------------------------------------------------
//  <copyright file="ICurrentPrincipalProvider.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Interfaces;

using System.Security.Principal;

public interface ICurrentPrincipalProvider
{
    IPrincipal? GetCurrentPrincipal();

    string? GetUserName();
}