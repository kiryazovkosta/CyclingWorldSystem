// ------------------------------------------------------------------------------------------------
//  <copyright file="UsersResponse.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Models;

public sealed record UserResponse(
    Guid Id, 
    string UserName, 
    string Email, 
    bool EmailConfirmed,
    string FirstName,
    string LastName,
    string ImageUrl,
    List<string> Roles);