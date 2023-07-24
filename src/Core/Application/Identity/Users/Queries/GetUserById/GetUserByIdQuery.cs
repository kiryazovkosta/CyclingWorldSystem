namespace Application.Identity.Users.Queries.GetUserById;

using Abstractions.Messaging;
using Models;

public sealed record GetUserByIdQuery(Guid Id) : IQuery<UserResponse>;