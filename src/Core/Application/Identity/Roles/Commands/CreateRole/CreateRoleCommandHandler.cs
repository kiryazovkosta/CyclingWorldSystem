// ------------------------------------------------------------------------------------------------
//  <copyright file="CreateRoleCommandHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Roles.Commands.CreateRole;

using Abstractions.Messaging;
using Domain.Errors;
using Domain.Identity;
using Domain.Shared;
using Microsoft.AspNetCore.Identity;

public class CreateRoleCommandHandler
    : ICommandHandler<CreateRoleCommand, Guid>
{
    private readonly RoleManager<Role> roleManager;

    public CreateRoleCommandHandler(RoleManager<Role> roleManager)
    {
        this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
    }

    public async Task<Result<Guid>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        bool roleExists = await this.roleManager.RoleExistsAsync(request.Name);
        if (roleExists)
        {
            return Result.Failure<Guid>(DomainErrors.Role.RoleAlreadyExists);
        }

        var role = new Role() { Name = request.Name };
        var result = await this.roleManager.CreateAsync(role);
        if (!result.Succeeded)
        {
            return Result.Failure<Guid>(DomainErrors.Role.FailedToCreate);
        }

        return Result.Success<Guid>(role.Id);
    }
}