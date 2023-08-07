// ------------------------------------------------------------------------------------------------
//  <copyright file="UpdateUserDetailsCommand.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Commands.UpdateUserDetails;

using Abstractions.Messaging;

public sealed record UpdateUserDetailsCommand(
    string UserName, 
    string Email,
    string FirstName,
    string LastName) : ICommand<bool>;