namespace Application.Identity.Users.Commands.UpdateUser;

using Abstractions.Messaging;
using DeleteUser;
using Domain.Errors;
using Domain.Identity;
using Domain.Shared;
using Microsoft.AspNetCore.Identity;

public class UpdateUserCommandHandler
    : ICommandHandler<UpdateUserCommand>
{
    private readonly UserManager<User> userManager;

    public UpdateUserCommandHandler(UserManager<User> userManager)
    {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userManager.FindByIdAsync(request.Id.ToString());
        if (user is null)
        {
            return Result.Failure(DomainErrors.DeleteOperationFailed(request.Id, nameof(DeleteUserCommand)));            
        }

        user.Update(request.UserName, request.Email, request.EmailConfirmed, request.FirstName, request.LastName);
        var result = await this.userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return Result.Failure(DomainErrors.User.FailedToUpdateUser);
        }

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            var token = await this.userManager.GeneratePasswordResetTokenAsync(user);

            var updateResult = await this.userManager.ResetPasswordAsync(user, token, request.Password);
            if (!updateResult.Succeeded)
            {
                return Result.Failure(DomainErrors.User.FailedToUpdatePassword);
            }
        }

        return Result.Success();
    }
}