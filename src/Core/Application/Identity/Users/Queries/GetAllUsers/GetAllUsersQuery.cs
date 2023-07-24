// ------------------------------------------------------------------------------------------------
//  <copyright file="GetAllUsersQuery.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Queries.GetAllUsers;

using Abstractions.Messaging;
using Models;

public sealed record GetAllUsersQuery() : IQuery<List<UserResponse>>;