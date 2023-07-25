// ------------------------------------------------------------------------------------------------
//  <copyright file="GetUserRolesCommandHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Identity.Users.Queries.GetUserRoles;

using Abstractions.Messaging;
using Domain.Errors;
using Domain.Identity;
using Domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class GetUserRolesCommandHandler
: ICommandHandler<GetUserRolesCommand, IEnumerable<string>>
{
    private readonly UserManager<User> userManager;

    public GetUserRolesCommandHandler(UserManager<User> userManager)
    {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<Result<IEnumerable<string>>> Handle(
        GetUserRolesCommand request, 
        CancellationToken cancellationToken)
    {
        var user = await this.userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Result.Failure<IEnumerable<string>>(DomainErrors.User.NonExistsUser);
        }

        var rolesList = this.userManager.Users.Where(u => u.Id == user.Id)
            .Select(usr => usr.UserRoles.Select(ur => ur.Role.Name!).ToList()).First().AsEnumerable();
        return Result.Success(rolesList);
    }
}