namespace Application.Identity.Users.Queries.GetAllUsers;

using Abstractions.Messaging;
using Domain.Identity;
using Domain.Identity.Dtos;
using Domain.Shared;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;

public class GetAllUsersQueryHandler
    : IQueryHandler<GetAllUsersQuery, List<UserResponse>>
{
    private readonly UserManager<User> userManager;

    public GetAllUsersQueryHandler(UserManager<User> userManager)
    {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<Result<List<UserResponse>>> Handle(
        GetAllUsersQuery request, 
        CancellationToken cancellationToken)
    {
        var users = await this.userManager.Users.Select(user => new UserDto()
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email!,
            EmailConfirmed = user.EmailConfirmed,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ImageUrl = user.ImageUrl,
            Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            
        })
        .ToListAsync(cancellationToken);

        var response = users.Adapt<List<UserResponse>>();

        return response;
    }
}