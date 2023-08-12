namespace Application.Identity.Users.Queries.GetAllUsers;

using Abstractions.Messaging;
using Domain.Identity;
using Domain.Identity.Dtos;
using Domain.Repositories;
using Domain.Shared;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;

public class GetAllUsersQueryHandler
    : IQueryHandler<GetAllUsersQuery, PagedUsersDataResponse>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        this._userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<Result<PagedUsersDataResponse>> Handle(
        GetAllUsersQuery request, 
        CancellationToken cancellationToken)
    {
        var users = await this._userRepository.GetUsers(request.Parameters, cancellationToken);
        var response = users.Adapt<PagedUsersDataResponse>();
        response.OrderBy = request.Parameters.OrderBy;
        response.FilterBy = request.Parameters.FilterBy;
        response.SearchBy = request.Parameters.SearchBy;
        return response;
    }
}