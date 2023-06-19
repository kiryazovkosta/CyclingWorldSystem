namespace Application.Identity.Users.Commands.CreateUser;

using Application.Abstractions.Messaging;
using Application.Identity.Users.Models;
using Domain.Identity;
using Domain.Repositories.Abstractions;
using Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

public sealed class CreateUserCommandHandler
	: ICommandHandler<CreateUserCommand, User>
{
	private readonly UserManager<User> _userManager;
	private readonly IUnitOfWork _context;

	public CreateUserCommandHandler(UserManager<User> userManager, IUnitOfWork context)
	{
		_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
	{
		if (request is null)
		{
			return Result.Failure<User>(Error.NullValue);
		}

		var dbUser = request.Adapt<User>();
		if (dbUser is null)
		{
			return Result.Failure<User>(Error.NullValue);
		}

		var result = await _userManager.CreateAsync(dbUser);
		if (result.Succeeded)
		{
			var user = await _userManager.FindByNameAsync(dbUser.UserName!);
			if (user is null)
			{
				return Result.Failure<User>(Error.NullValue);
			}

			await _context.SaveChangesAsync();
			return user;
		}

		return Result.Failure<User>(Error.NullValue);
	}
}
