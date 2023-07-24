namespace Application.Identity.Roles.Queries.GetAllRoles;

using Abstractions.Messaging;
using Domain.Identity;
using Domain.Identity.Dtos;
using Domain.Shared;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;

public class GetAllRolesQueryHandler
    : IQueryHandler<GetAllRolesQuery, List<RoleResponse>>
{
    private readonly RoleManager<Role> roleManager;

    public GetAllRolesQueryHandler(RoleManager<Role> roleManager)
    {
        this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
    }

    public async Task<Result<List<RoleResponse>>> Handle(
        GetAllRolesQuery request, 
        CancellationToken cancellationToken)
    {
        var roles = await this.roleManager.Roles
            .Select(role => new RoleDto()
            {
                Id = role.Id,
                Name = role.Name!,
                Users = role.UserRoles.Select(ur => ur.User.UserName).ToList()
            })
            .ToListAsync(cancellationToken);
            
        var response = roles.Adapt<List<RoleResponse>>();

        return response;
    }
}