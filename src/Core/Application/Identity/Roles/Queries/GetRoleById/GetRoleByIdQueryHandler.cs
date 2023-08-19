// ------------------------------------------------------------------------------------------------
//  <copyright file="GetRoleByIdQueryHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Roles.Queries.GetRoleById;

using Abstractions.Messaging;
using Domain.Errors;
using Domain.Identity;
using Domain.Shared;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Models;

public class GetRoleByIdQueryHandler
    : IQueryHandler<GetRoleByIdQuery, RoleResponse>
{
    private readonly RoleManager<Role> roleManager;

    public GetRoleByIdQueryHandler(RoleManager<Role> roleManager)
    {
        this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
    }

    public async Task<Result<RoleResponse>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await this.roleManager.FindByIdAsync(request.Id.ToString());
        if (role is null)
        {
            return Result.Failure<RoleResponse>(DomainErrors.Role.NonExistsRole);
        }

        var response = role.Adapt<RoleResponse>();
        return response;
    }
}