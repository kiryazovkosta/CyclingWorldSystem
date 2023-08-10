// ------------------------------------------------------------------------------------------------
//  <copyright file="GetAllActivitiesQuery.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Activities.Queries.GetAll;

using Abstractions.Messaging;
using Domain;
using Models;

public sealed record GetAllActivitiesQuery(QueryParameter Parameters) : IQuery<PagedActivityDataResponse>;