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
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Common.Constants;
using Domain.Errors;
using Microsoft.AspNetCore.WebUtilities;

public sealed class CreateUserCommandHandler
	: ICommandHandler<CreateUserCommand, Guid>
{
	private readonly UserManager<User> _userManager;
	private readonly ICloudinaryService _cloudinaryService;
	private readonly IEmailSender _emailSender;
	private readonly IUnitOfWork _context;

	public CreateUserCommandHandler(
		UserManager<User> userManager, 
		ICloudinaryService cloudinaryService,
		IEmailSender emailSender, 
		IUnitOfWork unitOfWork)
	{
		this._userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
		this._cloudinaryService = cloudinaryService ?? throw new ArgumentNullException(nameof(cloudinaryService));
		this._emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
		this._context = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
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

		if (request.Password != request.ConfirmPassword)
		{
			return Result.Failure<Guid>(DomainErrors.User.PasswordsAreNotEqual);
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

			var userId = await this._userManager.GetUserIdAsync(user);
			userId = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(userId));
			var code = await this._userManager.GenerateEmailConfirmationTokenAsync(user);
			code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
			var confirmUrl = $"http://localhost:5268/Account/ConfirmEmail?code={code}&userId={userId}";
			await this._emailSender.SendEmailAsync(
				request.Email, 
				"Confirm your email", 
				$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmUrl)}'>clicking here</a>.");

			await _context.SaveChangesAsync(cancellationToken);
			return user.Id;
		}

		return Result.Failure<Guid>(
			new Error("CreateUser", string.Join(". ", 
				result.Errors.Select(e => e.Description))));
	}
}
