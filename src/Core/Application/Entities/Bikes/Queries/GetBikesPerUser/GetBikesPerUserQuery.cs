using Application.Abstractions.Messaging;
using Application.Entities.Bikes.Models;

namespace Application.Entities.Bikes.Queries.GetBikes;

public sealed record GetBikesPerUserQuery(Guid UserId) : IQuery<List<BikeResponse>>;
