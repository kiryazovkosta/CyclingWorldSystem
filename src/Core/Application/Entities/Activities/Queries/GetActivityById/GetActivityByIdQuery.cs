namespace Application.Entities.Activities.Queries.GetActivityById;

using Abstractions.Messaging;
using Models;

public sealed record GetActivityByIdQuery(Guid Id) : IQuery<ActivityResponse>;