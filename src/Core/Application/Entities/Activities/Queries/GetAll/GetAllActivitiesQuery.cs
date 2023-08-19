namespace Application.Entities.Activities.Queries.GetAll;

using Abstractions.Messaging;
using Domain;
using Models;

public sealed record GetAllActivitiesQuery(QueryParameter Parameters) 
    : IQuery<PagedActivityDataResponse>;