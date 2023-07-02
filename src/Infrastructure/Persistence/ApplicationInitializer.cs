using Common.Constants;
using Domain.Identity;
using Domain.Repositories.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence;

public class ApplicationInitializer
{
	private readonly List<Role> roles = new List<Role>()
	{
		new Role
		{
			Name = GlobalConstants.Role.AdministratorRoleName,
			NormalizedName = GlobalConstants.Role.AdministratorRoleName.ToUpper()
		},
		new Role
		{
			Name = GlobalConstants.Role.ManagerRoleName,
			NormalizedName = GlobalConstants.Role.ManagerRoleName.ToUpper()
		},
		new Role
		{
			Name = GlobalConstants.Role.UserRoleName,
			NormalizedName = GlobalConstants.Role.UserRoleName.ToUpper()
		}
	};

	private readonly List<User> users = new List<User>()
	{
		new User
		{
			UserName = GlobalConstants.User.AdministratorUserName,
			NormalizedUserName = GlobalConstants.User.AdministratorUserName.ToUpper(),
			Email = GlobalConstants.User.AdministratorUserName + GlobalConstants.User.DefaultEmailDomain,
			FirstName = string.Empty,
			LastName = string.Empty,
			ImageUrl = string.Empty,
			EmailConfirmed = true,
			TwoFactorEnabled = false
		},
		new User
		{
			UserName = GlobalConstants.User.ManagerUserName,
			NormalizedUserName = GlobalConstants.User.ManagerUserName.ToUpper(),
			Email = GlobalConstants.User.ManagerUserName + GlobalConstants.User.DefaultEmailDomain,
			FirstName = string.Empty,
			LastName = string.Empty,
			ImageUrl = string.Empty,
			EmailConfirmed = true,
			TwoFactorEnabled = false
		},
		new User
		{
			UserName = GlobalConstants.User.UserUserName,
			NormalizedUserName = GlobalConstants.User.UserUserName.ToUpper(),
			Email = GlobalConstants.User.UserUserName + GlobalConstants.User.DefaultEmailDomain,
			FirstName = string.Empty,
			LastName = string.Empty,
			ImageUrl = string.Empty,
			EmailConfirmed = true,
			TwoFactorEnabled = false
		}
	};

	private readonly Dictionary<string, List<string>> usersRoles = new Dictionary<string, List<string>>()
	{
		{ 
			GlobalConstants.User.AdministratorUserName, 
			new List<string>(){ GlobalConstants.Role.AdministratorRoleName } 
		},
		{
			GlobalConstants.User.ManagerUserName,
			new List<string>(){ GlobalConstants.Role.ManagerRoleName }
		},
				{
			GlobalConstants.User.UserUserName,
			new List<string>(){ GlobalConstants.Role.UserRoleName }
		}
	};

	private readonly ApplicationDbContext _context;
	private readonly RoleManager<Role> _roleManager;
	private readonly UserManager<User> _userManager;
	private readonly IUnitOfWork _unitOfWork;

	public ApplicationInitializer(
		ApplicationDbContext context, 
		RoleManager<Role> roleManager, 
		UserManager<User> userManager, 
		IUnitOfWork unitOfWork)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
		_roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
		_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task SeedAsync()
	{
		await SeedRoles();
		await SeedUsers();
	}

	private async Task SeedUsers()
	{
		foreach (var user in users)
		{
			if (string.IsNullOrEmpty(user.UserName))
			{
				continue;
			}

			var dbUser = await _userManager.FindByNameAsync(user.UserName);
			if (dbUser is null)
			{
				await _userManager.CreateAsync(user, GlobalConstants.User.DefaultPassword);
			}

			dbUser = await _userManager.FindByNameAsync(user.UserName);
			if (dbUser is not null)
			{
				var newRoles = this.usersRoles[user.UserName]
					.Where(r => !this._userManager.IsInRoleAsync(dbUser, r).Result)
					.ToList();

				if (!newRoles.IsNullOrEmpty())
				{
					await this._userManager.AddToRolesAsync(dbUser, newRoles);
				}
			}
		}
	}

	private async Task SeedRoles()
	{
		foreach (var role in roles)
		{
			if (role.Name is null)
			{
				continue;
			}

			var dbRole = await _roleManager.FindByNameAsync(role.Name);
			if (dbRole is null)
			{
				await _roleManager.CreateAsync(role);
			}
		}
	}
}
