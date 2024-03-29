﻿// ------------------------------------------------------------------------------------------------
//  <copyright file="GetBikeTypeByIdQuery.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.BikeTypes.Queries.GetBikeTypeById;

using Abstractions.Messaging;
using Models;

public sealed record GetBikeTypeByIdQuery(Guid Id) : IQuery<SimpleBikeTypeResponse>;