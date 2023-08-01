namespace Application.Identity.Users.Commands.CreateUser;

using Application.Abstractions.Messaging;
using Application.Identity.Users.Models;
using Application.Interfaces;
using Domain.Identity;
using Domain.Repositories.Abstractions;
using Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Constants;

public sealed class CreateUserCommandHandler
	: ICommandHandler<CreateUserCommand, Guid>
{
	private readonly UserManager<User> _userManager;
	private readonly ICloudinaryService _cloudinaryService;
	private readonly IUnitOfWork _context;

    public CreateUserCommandHandler(
		UserManager<User> userManager, 
		ICloudinaryService cloudinaryService, 
		IUnitOfWork context)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _cloudinaryService = cloudinaryService ?? throw new ArgumentNullException(nameof(cloudinaryService));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Result<Guid>> Handle(
		CreateUserCommand request, 
		CancellationToken cancellationToken)
	{
		var dbUser = request.Adapt<User>();
		if (dbUser is null)
		{
			return Result.Failure<Guid>(Error.NullValue);
		}

		if (request.Avatar is not null)
		{
			var avatarUrl = await this._cloudinaryService.UploadAsync(request.Avatar);

			var end = avatarUrl.Substring(
			GlobalConstants.Cloudinary.AvatarBeginPath.Length, 
				avatarUrl.Length - GlobalConstants.Cloudinary.AvatarBeginPath.Length );

			dbUser.ImageUrl = $"{GlobalConstants.Cloudinary.AvatarBeginPath}{GlobalConstants.Cloudinary.AvatarRoundedAlgorithm}{end}";
		}

		var result = await _userManager.CreateAsync(dbUser, request.Password);
		if (result.Succeeded)
		{
			var user = await _userManager.FindByNameAsync(dbUser.UserName!);
			if (user is null)
			{
				return Result.Failure<Guid>(Error.NullValue);
			}

			await _context.SaveChangesAsync();
			return user.Id;
		}

		return Result.Failure<Guid>(Error.NullValue);
	}
}
