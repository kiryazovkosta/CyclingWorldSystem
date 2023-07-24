// ------------------------------------------------------------------------------------------------
//  <copyright file="UpdateRoleCommandHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Roles.Commands.UpdateRole;

using Abstractions.Messaging;
using Domain.Errors;
using Domain.Identity;
using Domain.Shared;
using Microsoft.AspNetCore.Identity;

public class UpdateRoleCommandHandler
    : ICommandHandler<UpdateRoleCommand>
{
    private readonly RoleManager<Role> roleManager;

    public UpdateRoleCommandHandler(RoleManager<Role> roleManager)
    {
        this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
    }

    public async Task<Result> Handle(
        UpdateRoleCommand request, 
        CancellationToken cancellationToken)
    {
        var role = await this.roleManager.FindByIdAsync(request.Id.ToString());
        if (role is null)
        {
            return Result.Failure(DomainErrors.Role.NonExistsRole);
        }

        if (await this.roleManager.RoleExistsAsync(request.Name))
        {
            return Result.Failure<Guid>(DomainErrors.Role.RoleAlreadyExists);
        }

        role.Name = request.Name;
        var result = await this.roleManager.UpdateAsync(role);
        if (!result.Succeeded)
        {
            return Result.Failure(DomainErrors.AnUnexpectedError(nameof(UpdateRoleCommand)));
        }

        return Result.Success();
    }
}