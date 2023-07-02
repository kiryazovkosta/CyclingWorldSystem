namespace Application.Identity.Users.Commands.LoginUser;

using Application.Abstractions;
using Application.Abstractions.Messaging;
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
	: ICommandHandler<LoginUserCommand, string>
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

	public async Task<Result<string>> Handle(
		LoginUserCommand request, 
		CancellationToken cancellationToken)
	{
		var user = await this._userManager.FindByNameAsync(request.UserName)
				  ?? throw new UserNotFoundException(request.UserName);
        if (user is not null && !user.IsDeleted)
        {
			var result = await this._signInManager.PasswordSignInAsync(
				request.UserName, 
				request.Password, 
				false, 
				false);

			if (result.Succeeded)
			{
				var token = this._jwkProvider.Generate(user);
				return Result.Create(token);
			}
		}

		if (!await this._userManager.IsEmailConfirmedAsync(user!))
		{
			return Result.Failure<string>(new Error("", ""));
		}

		return Result.Failure<string>(new Error("", ""));


	}
}
