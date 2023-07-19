namespace Application.Entities.Bikes.Queries.GetBikesPerUser;

using Application.Abstractions.Messaging;
using Application.Entities.Bikes.Models;

public sealed record GetBikesPerUserQuery(Guid UserId) : IQuery<List<BikeResponse>>;
