// ------------------------------------------------------------------------------------------------
//  <copyright file="GetAllUsersQuery.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Queries.GetAllUsers;

using Abstractions.Messaging;
using Domain;
using Models;

public sealed record GetAllUsersQuery(QueryParameter Parameters) 
    : IQuery<PagedUsersDataResponse>;