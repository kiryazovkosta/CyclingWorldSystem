// ------------------------------------------------------------------------------------------------
//  <copyright file="GetMineQuery.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Activities.Queries.GetMine;

using Abstractions.Messaging;
using Models;

public sealed record GetMineQuery(Guid UserId) : IQuery<List<MyActivityResponse>>;