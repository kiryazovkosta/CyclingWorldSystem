namespace Domain.Repositories;

using Identity.Dtos;
using Primitives;

public interface IUserRepository
{
    Task<IPagedList<UserDto, Guid>> GetUsers(
        QueryParameter parameters,
        CancellationToken cancellationToken = default);
}