namespace Application.Entities.Bikes.Queries.GetBikeById;

using Application.Abstractions.Messaging;
using Application.Entities.Bikes.Models;
using System;

public sealed record GetBikeByIdQuery(Guid Id) : IQuery<BikeResponse>;