// ------------------------------------------------------------------------------------------------
//  <copyright file="UserRepository.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Persistence.Repositories;

using Domain;
using Domain.Identity;
using Domain.Identity.Dtos;
using Domain.Primitives;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager)
    {
        this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<IPagedList<UserDto, Guid>> GetUsers(
        QueryParameter parameters,
        CancellationToken cancellationToken = default)
    {
        var users = await _userManager.Users
            .OrderBy(user => user.CreatedOn)
            .Select(user => new UserDto()
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
            .ToPagedListAsync<UserDto, Guid>(
                parameters.PageNumber,
                parameters.PageSize,
                cancellationToken);
        return users;
    }
}