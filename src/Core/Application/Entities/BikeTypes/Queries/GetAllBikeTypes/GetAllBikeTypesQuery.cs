namespace Application.Entities.BikeTypes.Queries.GetAllBikeTypes;

using Application.Abstractions.Messaging;
using Application.Entities.BikeTypes.Models;

public sealed record GetAllBikeTypesQuery : IQuery<IEnumerable<SimpleBikeTypeResponse>>;