namespace Application.Entities.Activities.Queries.GetMine;

using Abstractions.Messaging;
using Models;

public sealed record GetMineQuery(Guid UserId) : IQuery<List<MyActivityResponse>>;