namespace Application.Entities.BikeTypes.Queries.ExistsBikeTypeById;

using Application.Abstractions.Messaging;

public sealed record ExistsBikeTypeByIsQuery(Guid Id) : IQuery<bool>;
