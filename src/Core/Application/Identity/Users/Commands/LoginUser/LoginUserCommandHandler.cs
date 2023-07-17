namespace Application.Identity.Users.Commands.LoginUser;

using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Identity.Users.Commands.LogInUser;
using Application.Identity.Users.Models;
using Domain.Errors;
using Domain.Exceptions;
using Domain.Exceptions.Base;
using Domain.Identity;
using Domain.Shared;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

public class LoginUserCommandHandler
	: ICommandHandler<LogInUserCommand, LogInUserResponse>
{
	private readonly SignInManager<User> _signInManager;
	private readonly UserManager<User> _userManager;
	private readonly IJwtProvider _jwkProvider;

	public LoginUserCommandHandler(
		SignInManager<User> signInManager, 
		UserManager<User> userManager, 
		IJwtProvider jwkProvider)
	{
		_signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
		_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
		_jwkProvider = jwkProvider ?? throw new ArgumentNullException(nameof(jwkProvider));
	}

	public async Task<Result<LogInUserResponse>> Handle(
		LogInUserCommand request, 
		CancellationToken cancellationToken)
	{
		var user = await this._userManager.FindByNameAsync(request.UserName);
		if (user is null)
		{
			return Result.Failure<LogInUserResponse>(DomainErrors.User.LogInFailed);
		}

        if (!user.IsDeleted)
        {
			var result = await this._signInManager.PasswordSignInAsync(
				request.UserName, 
				request.Password, 
				false, 
				false);

			if (result.Succeeded)
			{
                var roles = await this._userManager.GetRolesAsync(user);

                var token = this._jwkProvider.CreateToken(user, roles);
				return Result.Success(new LogInUserResponse(user.UserName!, user!.Email!, token));
			}
		}

		if (!await this._userManager.IsEmailConfirmedAsync(user!))
		{
			return Result.Failure<LogInUserResponse>(DomainErrors.User.EmailIsNotConfirmed);
		}

		return Result.Failure<LogInUserResponse>(DomainErrors.AnUnexpectedError(nameof(LogInUserCommand)));


	}
}
