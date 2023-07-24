// ------------------------------------------------------------------------------------------------
//  <copyright file="DeleteRoleCommandHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Roles.Commands.DeleteRole;

using Abstractions.Messaging;
using Domain.Errors;
using Domain.Identity;
using Domain.Shared;
using Microsoft.AspNetCore.Identity;

public class DeleteRoleCommandHandler
    : ICommandHandler<DeleteRoleCommand>
{
    private readonly UserManager<User> userManager;
    private readonly RoleManager<Role> roleManager;

    public DeleteRoleCommandHandler(
        UserManager<User> userManager, 
        RoleManager<Role> roleManager)
    {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
    }

    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await this.roleManager.FindByIdAsync(request.Id.ToString());
        if (role is null)
        {
            return Result.Failure(DomainErrors.Role.NonExistsRole);
        }

        if ((await this.userManager.GetUsersInRoleAsync(role.Name!)).Any())
        {
            return Result.Failure(DomainErrors.Role.NonEmptyRole);
        }

        var result = await this.roleManager.DeleteAsync(role);
        if (!result.Succeeded)
        {
            return Result.Failure(DomainErrors.DeleteOperationFailed(request.Id, nameof(DeleteRoleCommand)));
        }

        return Result.Success();
    }
}